using System.Windows;
using System.Windows.Input;
using PDV.Commands;
using PDV.Services.Interfaces;
using System.Threading.Tasks;
using PDV.Services;
using PDV.Views;

namespace PDV.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        private readonly ILocalTenantService _localTenantService;

        private string _email;
        private string _senha;
        private string _errorMessage;
        private bool _isLoading;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                ClearError();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                _senha = value;
                OnPropertyChanged();
                ClearError();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotLoading));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsNotLoading => !IsLoading;

        public ICommand LoginCommand { get; }

        private readonly IRetaguardaSyncCoordinator _syncCoordinator;

        public LoginViewModel(IAuthenticationService authenticationService, IAlertService alertService, ILocalTenantService localTenantService, IRetaguardaSyncCoordinator syncCoordinator)
        {
            _authenticationService = authenticationService;
            _alertService = alertService;
            _localTenantService = localTenantService;
            _syncCoordinator = syncCoordinator;
            LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, CanExecuteLogin);
        }

        private bool CanExecuteLogin()
        {
            return !IsLoading;
        }

        private async Task ExecuteLoginAsync()
        {
            if (IsLoading) return;
            if (!ValidateInputs()) return;

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var success = await Task.Run(() => _authenticationService.AuthenticateAsync(Email, Senha));

                if (success)
                {
                    var session = _authenticationService.CurrentSession;
                    if (!SessaoLiberada(session))
                    {
                        if (session == null)
                        {
                            ErrorMessage = "Nao foi possivel carregar a sessao de autenticacao.";
                            _alertService.ShowWarning("Nao foi possivel carregar a sessao de autenticacao.");
                            return;
                        }

                        var emailConfirmacao = session.Usuario.Email;
                        bool? confirmacaoConcluida = false;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var confirmacao = new ConfirmacaoRegistroView(
                                _authenticationService,
                                _alertService,
                                session.Empresa.Cnpj,
                                session.Usuario.Login)
                            {
                                Owner = Application.Current.MainWindow
                            };
                            confirmacaoConcluida = confirmacao.ShowDialog();
                        });

                        if (confirmacaoConcluida != true)
                        {
                            ErrorMessage = "Cadastro ainda pendente de confirmacao.";
                            _alertService.ShowWarning("Cadastro ainda pendente de confirmacao.");
                            return;
                        }

                        var confirmado = await Task.Run(() => _authenticationService.AuthenticateAsync(emailConfirmacao, Senha));
                        session = _authenticationService.CurrentSession;
                        if (!confirmado)
                        {
                            ErrorMessage = "Cadastro confirmado. Informe a senha novamente e clique em Entrar.";
                            _alertService.ShowWarning("Cadastro confirmado. Informe a senha novamente e clique em Entrar.");
                            return;
                        }

                        if (!SessaoLiberada(session))
                        {
                            if (SessaoConfirmadaSemToken(session))
                            {
                                ErrorMessage = "Cadastro confirmado. Clique em Entrar novamente para iniciar a sessao.";
                                _alertService.ShowWarning("Cadastro confirmado. Clique em Entrar novamente para iniciar a sessao.");
                            }
                            else
                            {
                                ErrorMessage = "Nao foi possivel iniciar a sessao apos a confirmacao.";
                                _alertService.ShowWarning("Nao foi possivel iniciar a sessao apos a confirmacao.");
                            }
                            return;
                        }
                    }

                    if (session == null)
                    {
                        ErrorMessage = "Nao foi possivel carregar a sessao de autenticacao.";
                        _alertService.ShowWarning("Nao foi possivel carregar a sessao de autenticacao.");
                        return;
                    }

                    _localTenantService.GarantirTenantLocal(session);
                    _syncCoordinator.NotificarLoginRealizado();
                    await FinalizarLoginAsync();
                }
                else
                {
                    ErrorMessage = "Usuário ou senha inválidos";
                    _alertService.ShowError("Usuário ou senha inválidos. Por favor, tente novamente.");
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Erro ao conectar: {ex.Message}";
                _alertService.ShowError($"Erro ao conectar: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearError()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = string.Empty;
        }

        private static bool SessaoLiberada(Services.Retaguarda.RetaguardaAuthSession? session)
        {
            return session != null &&
                   !string.IsNullOrWhiteSpace(session.Token) &&
                   string.Equals(session.Empresa.Registrado, "S", System.StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(session.Usuario.Confirmado, "S", System.StringComparison.OrdinalIgnoreCase);
        }

        private static bool SessaoConfirmadaSemToken(Services.Retaguarda.RetaguardaAuthSession? session)
        {
            return session != null &&
                   string.Equals(session.Empresa.Registrado, "S", System.StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(session.Usuario.Confirmado, "S", System.StringComparison.OrdinalIgnoreCase);
        }

        private async Task FinalizarLoginAsync()
        {
            var nome = _authenticationService.CurrentSession?.Usuario?.Nome;
            _alertService.ShowSuccess($"Login realizado com sucesso! Bem-vindo, {(string.IsNullOrWhiteSpace(nome) ? Email : nome)}.");
            await Task.Delay(800);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.NavigateToMain();
                }
            });
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                _alertService.ShowWarning("Por favor, informe o e-mail.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Senha))
            {
                _alertService.ShowWarning("Por favor, informe a senha.");
                return false;
            }

            return true;
        }
    }
}

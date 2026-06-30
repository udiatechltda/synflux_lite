using PDV.Commands;
using PDV.Models;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using PDV.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PDV.ViewModels.Login
{
    public class CadastroInicialViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        private readonly Action? _onCancel;
        private readonly Action? _onCompleted;
        private string _cnpj = "11222333000181";
        private string _razaoSocial = "Empresa Teste PDV";
        private string _nomeFantasia = "Empresa Teste PDV";
        private string _email = "teste@local.com";
        private string _usuarioNome = "Administrador";
        private string _login = "admin";
        private string _senha = string.Empty;
        private string _confirmarSenha = string.Empty;
        private string _perfil = "Administrador";
        private string _mensagem = string.Empty;
        private bool _isLoading;
        private int _currentStepIndex;
        private const int TotalSteps = 3;
        private static readonly Brush ActiveStepBrush = new SolidColorBrush(Color.FromRgb(28, 62, 114));
        private static readonly Brush DoneStepBrush = new SolidColorBrush(Color.FromRgb(77, 166, 255));
        private static readonly Brush PendingStepBrush = new SolidColorBrush(Color.FromRgb(229, 234, 241));
        private static readonly Brush ActiveStepTextBrush = new SolidColorBrush(Color.FromRgb(28, 62, 114));
        private static readonly Brush PendingStepTextBrush = new SolidColorBrush(Color.FromRgb(138, 148, 166));

        public string Cnpj { get => _cnpj; set { _cnpj = value; OnPropertyChanged(); } }
        public string RazaoSocial { get => _razaoSocial; set { _razaoSocial = value; OnPropertyChanged(); } }
        public string NomeFantasia { get => _nomeFantasia; set { _nomeFantasia = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string UsuarioNome { get => _usuarioNome; set { _usuarioNome = value; OnPropertyChanged(); } }
        public string Login { get => _login; set { _login = value; OnPropertyChanged(); } }
        public string Senha { get => _senha; set { _senha = value; OnPropertyChanged(); } }
        public string ConfirmarSenha { get => _confirmarSenha; set { _confirmarSenha = value; OnPropertyChanged(); } }
        public string Perfil { get => _perfil; set { _perfil = value; OnPropertyChanged(); } }

        public string Mensagem
        {
            get => _mensagem;
            set
            {
                _mensagem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMensagem));
            }
        }

        public bool HasMensagem => !string.IsNullOrWhiteSpace(Mensagem);

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

        public int CurrentStepIndex
        {
            get => _currentStepIndex;
            private set
            {
                _currentStepIndex = value;
                OnStepChanged();
            }
        }

        public bool IsEmpresaStep => CurrentStepIndex == 0;
        public bool IsUsuarioStep => CurrentStepIndex == 1;
        public bool IsSegurancaStep => CurrentStepIndex == 2;
        public bool CanGoBack => CurrentStepIndex > 0 && !IsLoading;
        public bool IsLastStep => CurrentStepIndex == TotalSteps - 1;
        public bool IsNotLastStep => !IsLastStep;
        public string ProgressText => $"Etapa {CurrentStepIndex + 1} de {TotalSteps}";
        public string StepTitle => CurrentStepIndex switch
        {
            0 => "Dados da empresa",
            1 => "Primeiro usuário",
            _ => "Senha e conclusão"
        };
        public string StepSubtitle => CurrentStepIndex switch
        {
            0 => "Informe a identificação principal da empresa que usará o PDV.",
            1 => "Defina o contato de confirmação e o usuário administrador inicial.",
            _ => "Crie a senha de acesso e finalize para seguir à confirmação do registro."
        };

        public Brush StepEmpresaBrush => GetStepBrush(0);
        public Brush StepUsuarioBrush => GetStepBrush(1);
        public Brush StepSegurancaBrush => GetStepBrush(2);
        public Brush StepEmpresaTextBrush => GetStepTextBrush(0);
        public Brush StepUsuarioTextBrush => GetStepTextBrush(1);
        public Brush StepSegurancaTextBrush => GetStepTextBrush(2);

        public ICommand AvancarCommand { get; }
        public ICommand VoltarCommand { get; }
        public ICommand CadastrarCommand { get; }
        public ICommand CancelarCommand { get; }

        public CadastroInicialViewModel(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            Action? onCancel = null,
            Action? onCompleted = null)
        {
            _authenticationService = authenticationService;
            _alertService = alertService;
            _onCancel = onCancel;
            _onCompleted = onCompleted;

            AvancarCommand = new PDV.Commands.RelayCommand(_ => Avancar(), _ => !IsLoading && !IsLastStep);
            VoltarCommand = new PDV.Commands.RelayCommand(_ => Voltar(), _ => CanGoBack);
            CadastrarCommand = new AsyncRelayCommand(ExecuteCadastrarAsync, () => !IsLoading);
            CancelarCommand = new PDV.Commands.RelayCommand(_ => _onCancel?.Invoke());
        }

        private void Avancar()
        {
            if (!ValidarEtapaAtual())
                return;

            if (!IsLastStep)
            {
                Mensagem = string.Empty;
                CurrentStepIndex++;
            }
        }

        private void Voltar()
        {
            if (CanGoBack)
            {
                Mensagem = string.Empty;
                CurrentStepIndex--;
            }
        }

        private async Task ExecuteCadastrarAsync()
        {
            if (!Validar())
                return;

            IsLoading = true;
            Mensagem = string.Empty;

            try
            {
                var request = new RetaguardaCreateAccountRequest
                {
                    Cnpj = Cnpj,
                    RazaoSocial = RazaoSocial,
                    NomeFantasia = NomeFantasia,
                    Email = Email,
                    UsuarioNome = UsuarioNome,
                    Login = Login,
                    Senha = Senha,
                    Perfil = Perfil
                };

                var response = await Task.Run(() => _authenticationService.CreateAccountAsync(request));
                if (response == null)
                {
                    Mensagem = "Nao foi possivel cadastrar. Verifique se o usuario ja existe ou se a API esta ativa.";
                    _alertService.ShowAlert(Mensagem, AlertType.Warning);
                    return;
                }

                Mensagem = $"Cadastro criado para {response.Usuario.Login}. Confirme o registro para liberar a empresa.";
                _alertService.ShowAlert(Mensagem, AlertType.Success);
                var confirmacao = new ConfirmacaoRegistroView(_authenticationService, _alertService, response.Empresa.Cnpj, response.Usuario.Login)
                {
                    Owner = Application.Current.MainWindow
                };
                var confirmado = confirmacao.ShowDialog();
                if (confirmado == true)
                    _onCompleted?.Invoke();
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao cadastrar: {ex.Message}";
                _alertService.ShowAlert(Mensagem, AlertType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool Validar()
        {
            if (!ValidarEmpresa())
            {
                CurrentStepIndex = 0;
                return false;
            }

            if (!ValidarUsuario())
            {
                CurrentStepIndex = 1;
                return false;
            }

            if (!ValidarSeguranca())
            {
                CurrentStepIndex = 2;
                return false;
            }

            return true;
        }

        private bool ValidarEtapaAtual()
        {
            return CurrentStepIndex switch
            {
                0 => ValidarEmpresa(),
                1 => ValidarUsuario(),
                _ => ValidarSeguranca()
            };
        }

        private bool ValidarEmpresa()
        {
            if (string.IsNullOrWhiteSpace(Cnpj) || Cnpj.Trim().Length < 14)
                return Avisar("Informe o CNPJ da empresa.");

            if (string.IsNullOrWhiteSpace(RazaoSocial))
                return Avisar("Informe a razao social.");

            if (string.IsNullOrWhiteSpace(NomeFantasia))
                return Avisar("Informe o nome fantasia.");

            return true;
        }

        private bool ValidarUsuario()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return Avisar("Informe o e-mail de confirmacao.");

            if (string.IsNullOrWhiteSpace(UsuarioNome))
                return Avisar("Informe o nome do usuario.");

            if (string.IsNullOrWhiteSpace(Login))
                return Avisar("Informe o login.");

            if (string.IsNullOrWhiteSpace(Perfil))
                return Avisar("Informe o perfil.");

            return true;
        }

        private bool ValidarSeguranca()
        {
            if (string.IsNullOrWhiteSpace(Senha) || Senha.Length < 4)
                return Avisar("A senha deve possuir pelo menos 4 caracteres.");

            if (!string.Equals(Senha, ConfirmarSenha, StringComparison.Ordinal))
                return Avisar("As senhas nao coincidem.");

            return true;
        }

        private bool Avisar(string mensagem)
        {
            Mensagem = mensagem;
            _alertService.ShowAlert(mensagem, AlertType.Warning);
            return false;
        }

        private Brush GetStepBrush(int stepIndex)
        {
            if (CurrentStepIndex == stepIndex)
                return ActiveStepBrush;

            return CurrentStepIndex > stepIndex ? DoneStepBrush : PendingStepBrush;
        }

        private Brush GetStepTextBrush(int stepIndex)
        {
            return CurrentStepIndex >= stepIndex ? ActiveStepTextBrush : PendingStepTextBrush;
        }

        private void OnStepChanged()
        {
            OnPropertyChanged(nameof(CurrentStepIndex));
            OnPropertyChanged(nameof(IsEmpresaStep));
            OnPropertyChanged(nameof(IsUsuarioStep));
            OnPropertyChanged(nameof(IsSegurancaStep));
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(IsLastStep));
            OnPropertyChanged(nameof(IsNotLastStep));
            OnPropertyChanged(nameof(ProgressText));
            OnPropertyChanged(nameof(StepTitle));
            OnPropertyChanged(nameof(StepSubtitle));
            OnPropertyChanged(nameof(StepEmpresaBrush));
            OnPropertyChanged(nameof(StepUsuarioBrush));
            OnPropertyChanged(nameof(StepSegurancaBrush));
            OnPropertyChanged(nameof(StepEmpresaTextBrush));
            OnPropertyChanged(nameof(StepUsuarioTextBrush));
            OnPropertyChanged(nameof(StepSegurancaTextBrush));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

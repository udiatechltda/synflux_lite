using PDV.Commands;
using PDV.Models;
using PDV.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDV.ViewModels.Login
{
    public class ConfirmacaoRegistroViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        private readonly Window _window;
        private string _codigoConfirmacao = string.Empty;
        private string _mensagem;
        private bool _isLoading;
        private bool _aceitaTermos;
        private bool _empresaMei;
        private bool _pendenteDeConfirmacao;
        private bool _codigoConfirmado;
        private string _lastAutoConfirmCode = string.Empty;

        public string Cnpj { get; }
        public string Login { get; }

        public string CodigoConfirmacao
        {
            get => _codigoConfirmacao;
            set
            {
                _codigoConfirmacao = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Mensagem
        {
            get => _mensagem;
            set
            {
                _mensagem = value;
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

        public bool AceitaTermos
        {
            get => _aceitaTermos;
            set
            {
                _aceitaTermos = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool EmpresaMei
        {
            get => _empresaMei;
            set
            {
                _empresaMei = value;
                OnPropertyChanged();
            }
        }

        public bool PendenteDeConfirmacao
        {
            get => _pendenteDeConfirmacao;
            set
            {
                _pendenteDeConfirmacao = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NaoPendenteDeConfirmacao));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool NaoPendenteDeConfirmacao => !PendenteDeConfirmacao;

        public ICommand ConfirmarTermosCommand { get; }
        public ICommand ConfirmarCodigoCommand { get; }
        public ICommand ReenviarEmailCommand { get; }
        public ICommand FecharCommand { get; }

        public event Action? RegistroConfirmado;

        public ConfirmacaoRegistroViewModel(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            Window window,
            string cnpj,
            string login)
        {
            _authenticationService = authenticationService;
            _alertService = alertService;
            _window = window;
            Cnpj = SomenteDigitos(cnpj);
            Login = login;
            _mensagem = "Leia os termos, confirme as opções e solicite o código para liberar o uso do PDV.";

            ConfirmarTermosCommand = new AsyncRelayCommand(
                ConfirmarTermosAsync,
                () => !IsLoading && !PendenteDeConfirmacao);
            ConfirmarCodigoCommand = new AsyncRelayCommand(
                ConfirmarCodigoAsync,
                () => !IsLoading && PendenteDeConfirmacao && CodigoConfirmacao.Trim().Length == 32);
            ReenviarEmailCommand = new AsyncRelayCommand(
                ReenviarEmailAsync,
                () => !IsLoading && PendenteDeConfirmacao);
            FecharCommand = new RelayCommand(() => _window.Close());
        }

        public void TentarConfirmarAutomaticamente()
        {
            var code = CodigoConfirmacao.Trim();
            if (code.Length == 32 && !string.Equals(code, _lastAutoConfirmCode, StringComparison.OrdinalIgnoreCase))
            {
                _lastAutoConfirmCode = code;
                _ = ConfirmarCodigoAsync();
            }
        }

        private async Task ConfirmarTermosAsync()
        {
            if (!AceitaTermos)
            {
                Avisar("É necessário ler e aceitar os termos de uso.");
                return;
            }

            IsLoading = true;
            try
            {
                var enviado = await Task.Run(() => _authenticationService.SendRegistrationEmailAsync(Cnpj, Login));
                if (!enviado)
                {
                    Avisar("Não foi possível enviar o e-mail de confirmação. Verifique a API e o cadastro da empresa.");
                    return;
                }

                PendenteDeConfirmacao = true;
                Mensagem = "Pegue o código enviado para o e-mail cadastrado e informe abaixo. Verifique também SPAM/Lixo Eletrônico.";
                _alertService.ShowAlert("E-mail de confirmação enviado.", AlertType.Success);
            }
            catch (Exception ex)
            {
                Avisar($"Erro ao enviar e-mail: {ex.Message}", AlertType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ReenviarEmailAsync()
        {
            IsLoading = true;
            try
            {
                var enviado = await Task.Run(() => _authenticationService.SendRegistrationEmailAsync(Cnpj, Login));
                if (enviado)
                    _alertService.ShowAlert("E-mail reenviado com sucesso.", AlertType.Success);
                else
                    Avisar("Ocorreu um problema ao tentar reenviar o e-mail.");
            }
            catch (Exception ex)
            {
                Avisar($"Erro ao reenviar e-mail: {ex.Message}", AlertType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ConfirmarCodigoAsync()
        {
            if (_codigoConfirmado)
                return;

            var code = CodigoConfirmacao.Trim();
            if (code.Length != 32)
            {
                Avisar("Informe o código de confirmação com 32 caracteres.");
                return;
            }

            IsLoading = true;
            try
            {
                var confirmado = await Task.Run(() => _authenticationService.ConfirmRegistrationCodeAsync(Cnpj, Login, code));
                if (!confirmado)
                {
                    Avisar("Código inválido.");
                    return;
                }

                _codigoConfirmado = true;
                PendenteDeConfirmacao = false;
                Mensagem = $"Código confirmado com sucesso. Use {Cnpj}|{Login} no login.";
                _alertService.DismissAll();
                _alertService.ShowAlert("Código confirmado com sucesso. Sistema liberado.", AlertType.Success);
                MarcarDialogoConfirmado();
                await Task.Delay(1200);
                RegistroConfirmado?.Invoke();
            }
            catch (Exception ex)
            {
                Avisar($"Erro ao confirmar código: {ex.Message}", AlertType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Avisar(string mensagem, AlertType tipo = AlertType.Warning)
        {
            Mensagem = mensagem;
            _alertService.ShowAlert(mensagem, tipo);
        }

        private void MarcarDialogoConfirmado()
        {
            try
            {
                if (_window.IsVisible)
                    _window.DialogResult = true;
            }
            catch (InvalidOperationException)
            {
                // A janela pode ter sido aberta sem ShowDialog em testes automatizados.
            }
        }

        private static string SomenteDigitos(string value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }
    }
}

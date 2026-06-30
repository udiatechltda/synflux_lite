using PDV.Commands;
using PDV.Models;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PDV.ViewModels.Login
{
    public class EsqueceuSenhaViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        private readonly Action? _onCancel;
        private string _emailUsuario = string.Empty;
        private string _mensagem = string.Empty;
        private bool _isLoading;

        public string EmailUsuario
        {
            get => _emailUsuario;
            set
            {
                _emailUsuario = value;
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

        public ICommand ProximoCommand { get; }
        public ICommand CancelarCommand { get; }

        public EsqueceuSenhaViewModel(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            Action? onCancel = null)
        {
            _authenticationService = authenticationService;
            _alertService = alertService;
            _onCancel = onCancel;

            ProximoCommand = new AsyncRelayCommand(
                ExecuteProximoAsync,
                () => !IsLoading && !string.IsNullOrWhiteSpace(EmailUsuario));
            CancelarCommand = new PDV.Commands.RelayCommand(_ => _onCancel?.Invoke());
        }

        private async Task ExecuteProximoAsync()
        {
            if (IsLoading)
                return;

            IsLoading = true;
            Mensagem = string.Empty;

            try
            {
                var retorno = await Task.Run(() => _authenticationService.RecoverPasswordAsync(EmailUsuario));
                TratarRetorno(retorno);
            }
            catch (Exception ex)
            {
                Mensagem = $"Erro ao solicitar recuperacao: {ex.Message}";
                _alertService.ShowAlert(Mensagem, AlertType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void TratarRetorno(RetaguardaPasswordRecoveryResponse retorno)
        {
            if (!retorno.Sucesso)
            {
                Mensagem = retorno.Mensagem;
                _alertService.ShowAlert(Mensagem, AlertType.Error);
                return;
            }

            Mensagem = retorno.Mensagem;
            if (!string.IsNullOrWhiteSpace(retorno.SenhaTemporaria))
                Mensagem += $"{Environment.NewLine}Senha temporaria: {retorno.SenhaTemporaria}";

            _alertService.ShowAlert(Mensagem, AlertType.Success);
        }
    }
}

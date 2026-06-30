using PDV.Models;
using PDV.Services;
using PDV.Services.Interfaces;
using PDV.Utilities.Converters;
using System;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public event Action<string> MenuItemSelected;

        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAuthenticationService? _authenticationService;
        private readonly IAlertService? _alertService;
        private readonly PdvCashSessionState? _cashSessionState;

        private bool _isCashOpen;
        public bool IsCashOpen
        {
            get => _isCashOpen;
            private set
            {
                if (!SetProperty(ref _isCashOpen, value))
                    return;

                OnPropertyChanged(nameof(CashActionText));
            }
        }

        public string CashActionText => IsCashOpen ? "Fechar Caixa" : "Abrir Caixa";

        public ICommand SelectSideMenuItemCommand { get; }

        public MenuViewModel(
            IViewModelNavigationService? navigationService = null,
            IPdvOperationService? pdvService = null,
            IAuthenticationService? authenticationService = null,
            IAlertService? alertService = null,
            PdvCashSessionState? cashSessionState = null)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _authenticationService = authenticationService;
            _alertService = alertService;
            _cashSessionState = cashSessionState;
            SelectSideMenuItemCommand = new RelayCommand<string>(OnMenuSelected);

            IsCashOpen = _pdvService?.ExisteMovimentoAberto() == true;
            if (_cashSessionState != null)
                _cashSessionState.StateChanged += AtualizarEstadoCaixa;
        }

        private void AtualizarEstadoCaixa()
        {
            IsCashOpen = _pdvService?.ExisteMovimentoAberto() == true;
        }

        private void OnMenuSelected(string viewName)
        {
            if (!PdvAccessPolicy.CanAccessRoute(_authenticationService, viewName))
            {
                _alertService?.ShowAlert("Perfil operador sem permissao para acessar esta area.", AlertType.Warning);
                return;
            }

            if (_pdvService != null &&
                PdvAccessPolicy.RequiresOpenCash(viewName) &&
                !_pdvService.ExisteMovimentoAberto())
            {
                _alertService?.ShowAlert("Abra o caixa antes de acessar esta operacao.", AlertType.Warning);
                _navigationService?.NavigateTo("AberturaCaixa");
                return;
            }

            MenuItemSelected?.Invoke(viewName);
        }
    }
}

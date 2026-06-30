using PDV.Models;
using PDV.Services.Interfaces;
using System.Globalization;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class SangriaViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAlertService? _alertService;

        private decimal _total;
        public decimal Total
        {
            get => _total;
            set { _total = value; OnPropertyChanged(nameof(Total)); }
        }

        private string _valorTexto = string.Empty;
        public string ValorTexto
        {
            get => _valorTexto;
            set => SetProperty(ref _valorTexto, value);
        }

        private string _observacao = string.Empty;
        public string Observacao
        {
            get => _observacao;
            set => SetProperty(ref _observacao, value);
        }

        public ICommand VoltarCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand ConfirmarCommand { get; }

        public SangriaViewModel(IViewModelNavigationService navigationService, IPdvOperationService pdvService, IAlertService alertService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _alertService = alertService;
            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            CancelarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            ConfirmarCommand = new RelayCommand(Confirmar);
        }

        public SangriaViewModel()
        {
            VoltarCommand = new RelayCommand(() => { });
            CancelarCommand = new RelayCommand(() => { });
            ConfirmarCommand = new RelayCommand(() => { });
        }

        private void Confirmar()
        {
            try
            {
                if (!TryParseValor(ValorTexto, out var valor))
                {
                    _alertService?.ShowAlert("Informe um valor valido para a sangria.", AlertType.Warning);
                    return;
                }

                Total = valor;
                _pdvService?.RegistrarSangria(Total, Observacao);
                _alertService?.ShowAlert("Sangria registrada com sucesso.", AlertType.Success);
                _navigationService?.NavigateTo("Home");
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private static bool TryParseValor(string? texto, out decimal valor)
        {
            valor = 0;
            texto = (texto ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out valor) &&
                !decimal.TryParse(texto.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
            {
                return false;
            }

            return valor > 0;
        }
    }
}

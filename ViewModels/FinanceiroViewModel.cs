using System.Windows.Input;
using PDV.Commands;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class FinanceiroViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService _navigationService;

        public ICommand VoltarCommand { get; }

        public FinanceiroViewModel()
        {
            VoltarCommand = new RelayCommand(() => { });
        }

        public FinanceiroViewModel(IViewModelNavigationService navigationService)
        {
            _navigationService = navigationService;
            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
        }

        public void AbrirContasPagar()
        {
            _navigationService?.NavigateTo("ContasPagar");
        }

        public void AbrirContasReceber()
        {
            _navigationService?.NavigateTo("ContasReceber");
        }
    }
}
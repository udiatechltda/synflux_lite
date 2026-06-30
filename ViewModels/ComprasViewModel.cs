using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class ComprasViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;

        public ObservableCollection<CompraPedidoDto> Compras { get; } = new();

        public ICommand VoltarCommand { get; }
        public ICommand NovoPedidoCommand { get; }

        public ComprasViewModel(IViewModelNavigationService navigationService, IPdvOperationService pdvService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;

            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            NovoPedidoCommand = new RelayCommand(() => _navigationService.NavigateTo("ComprasForm"));
            Carregar();
        }

        public ComprasViewModel()
        {
            VoltarCommand = new RelayCommand(() => { });
            NovoPedidoCommand = new RelayCommand(() => { });
        }

        private void Carregar()
        {
            if (_pdvService == null)
                return;

            Compras.Clear();
            foreach (var compra in _pdvService.ListarCompras())
                Compras.Add(compra);
        }
    }
}

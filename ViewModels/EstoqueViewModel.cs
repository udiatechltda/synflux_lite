using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class EstoqueViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;

        public ObservableCollection<EstoqueItemDto> Produtos { get; } = new();
        public ObservableCollection<string> StatusDisponiveis { get; } = new() { "Todos", "Crítico" };

        private string _statusFiltro = "Todos";
        public string StatusFiltro
        {
            get => _statusFiltro;
            set
            {
                SetProperty(ref _statusFiltro, value);
                Carregar();
            }
        }

        public ICommand VoltarCommand { get; }
        public ICommand GerarPedidoCompraCommand { get; }

        public EstoqueViewModel(IViewModelNavigationService navigationService, IPdvOperationService pdvService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            GerarPedidoCompraCommand = new RelayCommand(() => _navigationService.NavigateTo("ComprasForm"));
            Carregar();
        }

        public EstoqueViewModel()
        {
            VoltarCommand = new RelayCommand(() => { });
            GerarPedidoCompraCommand = new RelayCommand(() => { });
        }

        private void Carregar()
        {
            if (_pdvService == null)
                return;

            Produtos.Clear();
            foreach (var produto in _pdvService.ListarEstoque(StatusFiltro))
                Produtos.Add(produto);
        }
    }
}

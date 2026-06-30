using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class NovoProdutoViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService _navigationService;

        private ObservableCollection<ProdutoImportado> _produtos;
        private ProdutoImportado _produtoSelecionado;
        private string _infoResultado;
        private int _paginaAtual = 1;

        public ObservableCollection<ProdutoImportado> Produtos
        {
            get => _produtos;
            set { _produtos = value; OnPropertyChanged(); }
        }

        public ProdutoImportado ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set { _produtoSelecionado = value; OnPropertyChanged(); }
        }

        public string InfoResultado
        {
            get => _infoResultado;
            set { _infoResultado = value; OnPropertyChanged(); }
        }

        public int PaginaAtual
        {
            get => _paginaAtual;
            set { _paginaAtual = value; OnPropertyChanged(); }
        }

        public ICommand FecharCommand { get; }
        public ICommand NovoProdutoCommand { get; }
        public ICommand SelecionarProdutoCommand { get; }
        public ICommand FiltroCommand { get; }
        public ICommand RelatorioCommand { get; }
        public ICommand PrimeiraPaginaCommand { get; }
        public ICommand PaginaAnteriorCommand { get; }
        public ICommand ProximaPaginaCommand { get; }
        public ICommand UltimaPaginaCommand { get; }

        public NovoProdutoViewModel(IViewModelNavigationService navigationService)
        {
            _navigationService = navigationService;
            Inicializar();
            FecharCommand = new RelayCommand(() => _navigationService.NavigateTo("ImportarProduto"));
            NovoProdutoCommand = new RelayCommand(ExecutarNovoProduto);
            SelecionarProdutoCommand = new RelayCommand(ExecutarSelecionarProduto);
            FiltroCommand = new RelayCommand(ExecutarFiltro);
            RelatorioCommand = new RelayCommand(ExecutarRelatorio);
            PrimeiraPaginaCommand = new RelayCommand(() => PaginaAtual = 1);
            PaginaAnteriorCommand = new RelayCommand(() => { if (PaginaAtual > 1) PaginaAtual--; });
            ProximaPaginaCommand = new RelayCommand(() => PaginaAtual++);
            UltimaPaginaCommand = new RelayCommand(() => PaginaAtual++);
        }

        public NovoProdutoViewModel()
        {
            _navigationService = App.NavigationService;
            Inicializar();
            FecharCommand = new RelayCommand(() => _navigationService?.NavigateTo("ImportarProduto"));
            NovoProdutoCommand = new RelayCommand(ExecutarNovoProduto);
            SelecionarProdutoCommand = new RelayCommand(ExecutarSelecionarProduto);
            FiltroCommand = new RelayCommand(ExecutarFiltro);
            RelatorioCommand = new RelayCommand(ExecutarRelatorio);
            PrimeiraPaginaCommand = new RelayCommand(() => PaginaAtual = 1);
            PaginaAnteriorCommand = new RelayCommand(() => { if (PaginaAtual > 1) PaginaAtual--; });
            ProximaPaginaCommand = new RelayCommand(() => PaginaAtual++);
            UltimaPaginaCommand = new RelayCommand(() => PaginaAtual++);
        }

        private void Inicializar()
        {
            Produtos = new ObservableCollection<ProdutoImportado>();
            InfoResultado = string.Empty;
            CarregarProdutosExemplo();
        }

        private void CarregarProdutosExemplo()
        {
            Produtos.Add(new ProdutoImportado
            {
                Id = 1,
                Gtin = "789123456001",
                CodigoInterno = "PROD001",
                Nome = "Produto Teste 1",
                Descricao = "Descrição do Produto Teste 1",
                ValorCompra = 10.00m,
                ValorVenda = 25.00m,
                Quantidade = 100
            });

            Produtos.Add(new ProdutoImportado
            {
                Id = 2,
                Gtin = "789123456002",
                CodigoInterno = "PROD002",
                Nome = "Produto Teste 2",
                Descricao = "Descrição do Produto Teste 2",
                ValorCompra = 15.00m,
                ValorVenda = 35.00m,
                Quantidade = 50
            });

            InfoResultado = $"{Produtos.Count} produto(s) encontrado(s).";
        }

        // ✅ Botão + navega para tela de inserção de produto
        private void ExecutarNovoProduto()
        {
            _navigationService?.NavigateTo("ProdutoInserindo");
        }

        private void ExecutarSelecionarProduto()
        {
            if (ProdutoSelecionado != null)
                _navigationService?.NavigateTo("ImportarProduto");
        }

        private void ExecutarFiltro()
        {
            System.Windows.MessageBox.Show("Abrir filtro de produtos", "Filtro",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void ExecutarRelatorio()
        {
            System.Windows.MessageBox.Show("Gerar relatório de produtos", "Relatório",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
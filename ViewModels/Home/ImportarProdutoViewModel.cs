using PDV.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class ImportarProdutoViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService _navigationService;

        public event EventHandler<ProdutoImportado> ProdutoSelecionado;

        private ObservableCollection<ProdutoImportado> _produtos;
        private ProdutoImportado _produtoSelecionadoItem;
        private string _termoPesquisa;
        private string _campoBuscaSelecionado;
        private string _infoResultado;
        private bool _mostrarDica;

        public ObservableCollection<string> CamposBusca { get; set; }

        public ObservableCollection<ProdutoImportado> Produtos
        {
            get => _produtos;
            set { _produtos = value; OnPropertyChanged(); }
        }

        public ProdutoImportado ProdutoSelecionadoItem
        {
            get => _produtoSelecionadoItem;
            set { _produtoSelecionadoItem = value; OnPropertyChanged(); }
        }

        public string TermoPesquisa
        {
            get => _termoPesquisa;
            set { _termoPesquisa = value; OnPropertyChanged(); }
        }

        public string CampoBuscaSelecionado
        {
            get => _campoBuscaSelecionado;
            set { _campoBuscaSelecionado = value; OnPropertyChanged(); }
        }

        public string InfoResultado
        {
            get => _infoResultado;
            set { _infoResultado = value; OnPropertyChanged(); }
        }

        public bool MostrarDica
        {
            get => _mostrarDica;
            set { _mostrarDica = value; OnPropertyChanged(); }
        }

        public ICommand FecharCommand { get; }
        public ICommand PesquisarCommand { get; }
        public ICommand NovoProdutoCommand { get; }
        public ICommand SelecionarProdutoCommand { get; }

        public ImportarProdutoViewModel(IViewModelNavigationService navigationService)
        {
            _navigationService = navigationService;
            Inicializar();
            FecharCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            PesquisarCommand = new RelayCommand(ExecutarPesquisar);
            NovoProdutoCommand = new RelayCommand(ExecutarNovoProduto);
            SelecionarProdutoCommand = new RelayCommand(ExecutarSelecionarProduto);
        }

        public ImportarProdutoViewModel()
        {
            _navigationService = App.NavigationService;

            Inicializar();
            FecharCommand = new RelayCommand(() => _navigationService?.NavigateTo("Home"));
            PesquisarCommand = new RelayCommand(ExecutarPesquisar);
            NovoProdutoCommand = new RelayCommand(ExecutarNovoProduto);
            SelecionarProdutoCommand = new RelayCommand(ExecutarSelecionarProduto);
        }

        private void Inicializar()
        {
            Produtos = new ObservableCollection<ProdutoImportado>();
            CamposBusca = new ObservableCollection<string> { "Nome", "GTIN", "Código Interno" };
            CampoBuscaSelecionado = "Nome";
            MostrarDica = true;
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
        }

        private void ExecutarPesquisar()
        {
            if (string.IsNullOrWhiteSpace(TermoPesquisa) || TermoPesquisa.Length < 3)
            {
                MostrarDica = true;
                InfoResultado = "Digite 3 ou mais caracteres para pesquisar.";
                return;
            }

            MostrarDica = false;
            InfoResultado = $"Encontrados {Produtos.Count} produto(s).";
        }

        private void ExecutarNovoProduto()
        {
            _navigationService?.NavigateTo("NovoProduto");
        }

        private void ExecutarSelecionarProduto()
        {
            if (ProdutoSelecionadoItem != null)
            {
                ProdutoSelecionado?.Invoke(this, ProdutoSelecionadoItem);
                _navigationService?.NavigateTo("Home");
            }
        }
    }
}
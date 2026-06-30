using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDV.ViewModels.NovoProduto
{
    public class ProdutoInserindoViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService _navigationService;

        // ── Campos ──────────────────────────────────────────────

        private string _unidade = string.Empty;
        private string _tipo = string.Empty;
        private string _subGrupo = string.Empty;
        private string _gtin = string.Empty;
        private string _codigoInterno = string.Empty;
        private string _nome = string.Empty;
        private string _descricao = string.Empty;
        private string _codigoNcm = string.Empty;
        private string _codigoCest = string.Empty;
        private string _ippt = string.Empty;
        private decimal _valorCompra;
        private decimal _valorVenda;
        private decimal _valorCusto;
        private decimal _quantidadeEstoque;
        private decimal _estoqueMinimo;
        private decimal _estoqueMaximo;

        // ── Erros de validação ───────────────────────────────────

        private bool _hasUnidadeError;
        private bool _hasTipoError;
        private bool _hasSubGrupoError;
        private bool _hasGtinError;
        private bool _hasNomeError;
        private bool _hasCodigoNcmError;
        private bool _hasValorVendaError;

        // ── Properties ──────────────────────────────────────────

        public ObservableCollection<string> OpcoesIppt { get; } = new()
        {
            "P - Própria",
            "T - Terceiros"
        };

        public string Unidade
        {
            get => _unidade;
            set { _unidade = value; OnPropertyChanged(); HasUnidadeError = false; }
        }

        public string Tipo
        {
            get => _tipo;
            set { _tipo = value; OnPropertyChanged(); HasTipoError = false; }
        }

        public string SubGrupo
        {
            get => _subGrupo;
            set { _subGrupo = value; OnPropertyChanged(); HasSubGrupoError = false; }
        }

        public string Gtin
        {
            get => _gtin;
            set { _gtin = value; OnPropertyChanged(); HasGtinError = false; }
        }

        public string CodigoInterno
        {
            get => _codigoInterno;
            set { _codigoInterno = value; OnPropertyChanged(); }
        }

        public string Nome
        {
            get => _nome;
            set { _nome = value; OnPropertyChanged(); HasNomeError = false; }
        }

        public string Descricao
        {
            get => _descricao;
            set { _descricao = value; OnPropertyChanged(); }
        }

        public string CodigoNcm
        {
            get => _codigoNcm;
            set { _codigoNcm = value; OnPropertyChanged(); HasCodigoNcmError = false; }
        }

        public string CodigoCest
        {
            get => _codigoCest;
            set { _codigoCest = value; OnPropertyChanged(); }
        }

        public string Ippt
        {
            get => _ippt;
            set { _ippt = value; OnPropertyChanged(); }
        }

        public decimal ValorCompra
        {
            get => _valorCompra;
            set { _valorCompra = value; OnPropertyChanged(); }
        }

        public decimal ValorVenda
        {
            get => _valorVenda;
            set { _valorVenda = value; OnPropertyChanged(); HasValorVendaError = false; }
        }

        public decimal ValorCusto
        {
            get => _valorCusto;
            set { _valorCusto = value; OnPropertyChanged(); }
        }

        public decimal QuantidadeEstoque
        {
            get => _quantidadeEstoque;
            set { _quantidadeEstoque = value; OnPropertyChanged(); }
        }

        public decimal EstoqueMinimo
        {
            get => _estoqueMinimo;
            set { _estoqueMinimo = value; OnPropertyChanged(); }
        }

        public decimal EstoqueMaximo
        {
            get => _estoqueMaximo;
            set { _estoqueMaximo = value; OnPropertyChanged(); }
        }

        // ── Erros ───────────────────────────────────────────────

        public bool HasUnidadeError
        {
            get => _hasUnidadeError;
            set { _hasUnidadeError = value; OnPropertyChanged(); }
        }

        public bool HasTipoError
        {
            get => _hasTipoError;
            set { _hasTipoError = value; OnPropertyChanged(); }
        }

        public bool HasSubGrupoError
        {
            get => _hasSubGrupoError;
            set { _hasSubGrupoError = value; OnPropertyChanged(); }
        }

        public bool HasGtinError
        {
            get => _hasGtinError;
            set { _hasGtinError = value; OnPropertyChanged(); }
        }

        public bool HasNomeError
        {
            get => _hasNomeError;
            set { _hasNomeError = value; OnPropertyChanged(); }
        }

        public bool HasCodigoNcmError
        {
            get => _hasCodigoNcmError;
            set { _hasCodigoNcmError = value; OnPropertyChanged(); }
        }

        public bool HasValorVendaError
        {
            get => _hasValorVendaError;
            set { _hasValorVendaError = value; OnPropertyChanged(); }
        }

        // ── Commands ────────────────────────────────────────────

        public ICommand FecharCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand BuscarUnidadeCommand { get; }
        public ICommand BuscarTipoCommand { get; }
        public ICommand BuscarSubGrupoCommand { get; }

        // ── Construtores ────────────────────────────────────────

        public ProdutoInserindoViewModel(IViewModelNavigationService navigationService)
        {
            _navigationService = navigationService;
            FecharCommand = new RelayCommand(ExecutarFechar);
            CancelarCommand = new RelayCommand(ExecutarFechar);
            SalvarCommand = new RelayCommand(ExecutarSalvar);
            BuscarUnidadeCommand = new RelayCommand(() => { /* TODO: busca de unidade */ });
            BuscarTipoCommand = new RelayCommand(() => { /* TODO: busca de tipo */ });
            BuscarSubGrupoCommand = new RelayCommand(() => { /* TODO: busca de subgrupo */ });
        }

        public ProdutoInserindoViewModel()
        {
            _navigationService = App.NavigationService;
            FecharCommand = new RelayCommand(ExecutarFechar);
            CancelarCommand = new RelayCommand(ExecutarFechar);
            SalvarCommand = new RelayCommand(ExecutarSalvar);
            BuscarUnidadeCommand = new RelayCommand(() => { });
            BuscarTipoCommand = new RelayCommand(() => { });
            BuscarSubGrupoCommand = new RelayCommand(() => { });
        }

        // ── Métodos ─────────────────────────────────────────────

        // Voltar para a tela de listagem de produtos
        private void ExecutarFechar()
        {
            _navigationService?.NavigateTo("NovoProduto");
        }

        private bool Validar()
        {
            HasUnidadeError = string.IsNullOrWhiteSpace(Unidade);
            HasTipoError = string.IsNullOrWhiteSpace(Tipo);
            HasSubGrupoError = string.IsNullOrWhiteSpace(SubGrupo);
            HasGtinError = string.IsNullOrWhiteSpace(Gtin);
            HasNomeError = string.IsNullOrWhiteSpace(Nome);
            HasCodigoNcmError = string.IsNullOrWhiteSpace(CodigoNcm);
            HasValorVendaError = ValorVenda <= 0;

            return !HasUnidadeError && !HasTipoError && !HasSubGrupoError &&
                   !HasGtinError && !HasNomeError && !HasCodigoNcmError && !HasValorVendaError;
        }

        private void ExecutarSalvar()
        {
            if (!Validar()) return;

            // TODO: chamar serviço para persistir o produto
            System.Windows.MessageBox.Show("Produto salvo com sucesso!", "Sucesso",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            _navigationService?.NavigateTo("NovoProduto");
        }
    }
}
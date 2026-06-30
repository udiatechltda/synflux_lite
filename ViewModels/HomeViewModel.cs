using PDV.Models;
using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAlertService? _alertService;

        public ObservableCollection<ItemVenda> Itens { get; } = new();
        public ObservableCollection<ItemVenda> ProdutosEncontrados { get; } = new();

        private int _totalItens;
        public int TotalItens
        {
            get => _totalItens;
            set { _totalItens = value; OnPropertyChanged(); }
        }

        private decimal _totalVenda;
        public decimal TotalVenda
        {
            get => _totalVenda;
            set { _totalVenda = value; OnPropertyChanged(); }
        }

        private string _termoBusca = string.Empty;
        public string TermoBusca
        {
            get => _termoBusca;
            set { _termoBusca = value; OnPropertyChanged(); }
        }

        private string _infoResultado = "Pesquise por GTIN, codigo interno ou descricao.";
        public string InfoResultado
        {
            get => _infoResultado;
            set { _infoResultado = value; OnPropertyChanged(); }
        }

        private ItemVenda? _produtoSelecionado;
        public ItemVenda? ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                _produtoSelecionado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProdutoSelecionadoImagemPath));
            }
        }

        public string? ProdutoSelecionadoImagemPath => ProdutoSelecionado?.ImagemLocalPath;

        private ItemVenda? _itemSelecionado;
        public ItemVenda? ItemSelecionado
        {
            get => _itemSelecionado;
            set { _itemSelecionado = value; OnPropertyChanged(); }
        }

        private bool _mostrarMontagemVenda = true;
        public bool MostrarMontagemVenda
        {
            get => _mostrarMontagemVenda;
            set { _mostrarMontagemVenda = value; OnPropertyChanged(); }
        }

        private bool _mostrarPagamento;
        public bool MostrarPagamento
        {
            get => _mostrarPagamento;
            set { _mostrarPagamento = value; OnPropertyChanged(); }
        }

        private string _tipoPagamentoSelecionado = "Dinheiro";
        public string TipoPagamentoSelecionado
        {
            get => _tipoPagamentoSelecionado;
            set
            {
                _tipoPagamentoSelecionado = value;
                OnPropertyChanged();
                AtualizarPagamento();
            }
        }

        private string _valorRecebidoTexto = string.Empty;
        public string ValorRecebidoTexto
        {
            get => _valorRecebidoTexto;
            set
            {
                _valorRecebidoTexto = value;
                OnPropertyChanged();
                AtualizarPagamento();
            }
        }

        private DateTime? _dataVencimento = DateTime.Today.AddDays(30);
        public DateTime? DataVencimento
        {
            get => _dataVencimento;
            set { _dataVencimento = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> TiposPagamento { get; } = new()
        {
            "Dinheiro",
            "Pix",
            "Cartao Debito",
            "Cartao Credito",
            "Vale Alimentacao",
            "A Prazo",
            "Fiado"
        };

        public string TotalVendaTexto => TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public string TrocoTexto => CalcularTroco().ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public string ValorAReceberTexto => TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public bool MostrarVencimentoFiadoPrazo => false;

        public ICommand AbrirImportarProdutoCommand { get; }
        public ICommand PesquisarCommand { get; }
        public ICommand AdicionarProdutoCommand { get; }
        public ICommand EncerrarVendaCommand { get; }
        public ICommand ConcluirVendaCommand { get; }
        public ICommand ConfirmarPagamentoCommand { get; }
        public ICommand VoltarPagamentoCommand { get; }

        public HomeViewModel(
            IViewModelNavigationService navigationService,
            IPdvOperationService pdvService,
            IAlertService alertService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _alertService = alertService;

            AbrirImportarProdutoCommand = new RelayCommand(() => _navigationService.NavigateTo("ImportarProduto"));
            PesquisarCommand = new RelayCommand(ExecutarPesquisar);
            AdicionarProdutoCommand = new RelayCommand(ExecutarAdicionarProduto);
            ConcluirVendaCommand = new RelayCommand(ExecutarConcluirVenda);
            ConfirmarPagamentoCommand = new RelayCommand(ExecutarConfirmarPagamento);
            VoltarPagamentoCommand = new RelayCommand(VoltarParaMontagem);
            EncerrarVendaCommand = ConcluirVendaCommand;
        }

        public HomeViewModel()
        {
            AbrirImportarProdutoCommand = new RelayCommand(() => { });
            PesquisarCommand = new RelayCommand(ExecutarPesquisar);
            AdicionarProdutoCommand = new RelayCommand(ExecutarAdicionarProduto);
            ConcluirVendaCommand = new RelayCommand(ExecutarConcluirVenda);
            ConfirmarPagamentoCommand = new RelayCommand(ExecutarConfirmarPagamento);
            VoltarPagamentoCommand = new RelayCommand(VoltarParaMontagem);
            EncerrarVendaCommand = ConcluirVendaCommand;
        }

        private void ExecutarPesquisar()
        {
            ProdutosEncontrados.Clear();

            if (string.IsNullOrWhiteSpace(TermoBusca) || TermoBusca.Trim().Length < 2)
            {
                InfoResultado = "Digite ao menos 2 caracteres para pesquisar.";
                return;
            }

            if (_pdvService == null)
            {
                InfoResultado = "Servico de venda indisponivel.";
                return;
            }

            var produtos = _pdvService.BuscarProdutos(TermoBusca);
            foreach (var produto in produtos)
            {
                ProdutosEncontrados.Add(new ItemVenda
                {
                    ProdutoId = produto.Id,
                    Gtin = produto.Gtin,
                    CodigoInterno = produto.CodigoInterno,
                    Descricao = produto.Descricao,
                    ValorUnitario = produto.ValorVenda,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    ImagemLocalPath = produto.ImagemLocalPath,
                    Quantidade = 1,
                    ValorTotal = produto.ValorVenda
                });
            }

            ProdutoSelecionado = ProdutosEncontrados.FirstOrDefault();
            InfoResultado = produtos.Count == 0
                ? "Nenhum produto encontrado."
                : $"{produtos.Count} produto(s) encontrado(s). Selecione e confirme para adicionar.";
        }

        private void ExecutarAdicionarProduto()
        {
            var selecionado = ProdutoSelecionado ?? ProdutosEncontrados.FirstOrDefault();
            if (selecionado == null)
            {
                _alertService?.ShowAlert("Pesquise e selecione um produto antes de adicionar.", AlertType.Warning);
                return;
            }

            var existente = Itens.FirstOrDefault(i => i.ProdutoId == selecionado.ProdutoId && i.Gtin == selecionado.Gtin);

            if (existente != null)
            {
                existente.Quantidade++;
                existente.ValorTotal = existente.Quantidade * existente.ValorUnitario - existente.Desconto;
            }
            else
            {
                Itens.Add(new ItemVenda
                {
                    ProdutoId = selecionado.ProdutoId,
                    Item = Itens.Count + 1,
                    Gtin = selecionado.Gtin,
                    CodigoInterno = selecionado.CodigoInterno,
                    Descricao = selecionado.Descricao,
                    ValorUnitario = selecionado.ValorUnitario,
                    Quantidade = 1,
                    QuantidadeEstoque = selecionado.QuantidadeEstoque,
                    ImagemLocalPath = selecionado.ImagemLocalPath,
                    Desconto = 0,
                    ValorTotal = selecionado.ValorUnitario
                });
            }

            ReordenarItens();
            CalcularTotal();
            FecharPagamentoSeAberto();
            TermoBusca = string.Empty;
            ProdutosEncontrados.Clear();
            ProdutoSelecionado = null;
            InfoResultado = "Produto adicionado a venda.";
        }

        private void ExecutarConcluirVenda()
        {
            if (Itens.Count == 0)
            {
                _alertService?.ShowAlert("Adicione ao menos um item antes de concluir a venda.", AlertType.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ValorRecebidoTexto))
                ValorRecebidoTexto = TotalVenda.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));

            MostrarMontagemVenda = false;
            MostrarPagamento = true;
            InfoResultado = "Informe a forma de pagamento e confirme a venda.";
            AtualizarPagamento();
        }

        private void ExecutarConfirmarPagamento()
        {
            try
            {
                if (Itens.Count == 0)
                {
                    _alertService?.ShowAlert("Adicione ao menos um item antes de concluir a venda.", AlertType.Warning);
                    VoltarParaMontagem();
                    return;
                }

                if (_pdvService == null)
                {
                    _alertService?.ShowAlert("Servico de venda indisponivel.", AlertType.Error);
                    return;
                }

                if (!TryParseValor(ValorRecebidoTexto, out var valorRecebido))
                {
                    _alertService?.ShowAlert("Informe um valor recebido valido.", AlertType.Warning);
                    return;
                }

                var fiado = EhPagamentoPendente(TipoPagamentoSelecionado);
                if (fiado)
                {
                    _alertService?.ShowAlert("Venda a prazo ou fiado exige cliente cadastrado. Use a tela de vendas com cliente.", AlertType.Warning);
                    return;
                }

                if (!fiado && valorRecebido < TotalVenda)
                {
                    _alertService?.ShowAlert("Valor recebido nao pode ser menor que o total da venda.", AlertType.Warning);
                    return;
                }

                var pagamento = new VendaPagamentoDto(
                    TipoPagamentoSelecionado,
                    fiado ? TotalVenda : valorRecebido,
                    fiado ? DataVencimento : null);

                var venda = _pdvService.FinalizarVenda(new VendaInput(
                    Itens.Select(i => new VendaItemDto(i.ProdutoId, i.Gtin, i.Descricao, i.Quantidade, i.ValorUnitario, i.Desconto)).ToList(),
                    new[] { pagamento },
                    ClienteFiado: fiado));

                Itens.Clear();
                ValorRecebidoTexto = string.Empty;
                VoltarParaMontagem();
                CalcularTotal();
                InfoResultado = $"Venda #{venda.Id} finalizada.";
                _alertService?.ShowAlert($"Venda finalizada com sucesso. Total: {venda.ValorFinal:C}", AlertType.Success);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void VoltarParaMontagem()
        {
            MostrarPagamento = false;
            MostrarMontagemVenda = true;
        }

        private void FecharPagamentoSeAberto()
        {
            if (MostrarPagamento)
                VoltarParaMontagem();
        }

        private void ReordenarItens()
        {
            for (int i = 0; i < Itens.Count; i++)
                Itens[i].Item = i + 1;
        }

        private void CalcularTotal()
        {
            TotalVenda = Itens.Sum(item => item.ValorTotal);
            TotalItens = Itens.Sum(item => (int)item.Quantidade);
            OnPropertyChanged(nameof(TotalVendaTexto));
            AtualizarPagamento();
        }

        private void AtualizarPagamento()
        {
            OnPropertyChanged(nameof(ValorAReceberTexto));
            OnPropertyChanged(nameof(TrocoTexto));
            OnPropertyChanged(nameof(MostrarVencimentoFiadoPrazo));
        }

        private decimal CalcularTroco()
        {
            if (!TryParseValor(ValorRecebidoTexto, out var recebido))
                return 0;

            var troco = recebido - TotalVenda;
            return troco > 0 ? troco : 0;
        }

        private static bool TryParseValor(string? texto, out decimal valor)
        {
            valor = 0;
            texto = (texto ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out valor)
                || decimal.TryParse(texto.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }

        private static bool EhPagamentoPendente(string tipoPagamento)
        {
            return tipoPagamento.Contains("Fiado", StringComparison.OrdinalIgnoreCase)
                || tipoPagamento.Contains("Prazo", StringComparison.OrdinalIgnoreCase)
                || tipoPagamento.Contains("A prazo", StringComparison.OrdinalIgnoreCase);
        }
    }

    public class ItemVenda
    {
        public int? ProdutoId { get; set; }
        public int Item { get; set; }
        public string Gtin { get; set; } = string.Empty;
        public string CodigoInterno { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeEstoque { get; set; }
        public string? ImagemLocalPath { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class ProdutoImportado
    {
        public int Id { get; set; }
        public string Gtin { get; set; } = string.Empty;
        public string CodigoInterno { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorCompra { get; set; }
        public decimal ValorVenda { get; set; }
        public int Quantidade { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _execute();
    }
}

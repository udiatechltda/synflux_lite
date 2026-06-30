using PDV.Models;
using PDV.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class VendasViewModel : ViewModelBase
    {
        private readonly IPdvOperationService? _pdvService;
        private readonly IAlertService? _alertService;

        public ObservableCollection<VendaResumoDto> Vendas { get; } = new();
        public ObservableCollection<ProdutoVendaDto> ProdutosEncontrados { get; } = new();
        public ObservableCollection<VendaItemLinha> ItensVenda { get; } = new();
        public ObservableCollection<string> StatusDisponiveis { get; } = new() { "Todas", "Fechadas", "Abertas", "Canceladas" };
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

        private string _termoProduto = string.Empty;
        public string TermoProduto
        {
            get => _termoProduto;
            set => SetProperty(ref _termoProduto, value);
        }

        private ProdutoVendaDto? _produtoSelecionado;
        public ProdutoVendaDto? ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                if (SetProperty(ref _produtoSelecionado, value))
                    OnPropertyChanged(nameof(ProdutoSelecionadoImagemPath));
            }
        }

        public string? ProdutoSelecionadoImagemPath => ProdutoSelecionado?.ImagemLocalPath;

        private VendaItemLinha? _itemSelecionado;
        public VendaItemLinha? ItemSelecionado
        {
            get => _itemSelecionado;
            set => SetProperty(ref _itemSelecionado, value);
        }

        private string _quantidadeTexto = "1";
        public string QuantidadeTexto
        {
            get => _quantidadeTexto;
            set => SetProperty(ref _quantidadeTexto, value);
        }

        private string _valorRecebidoTexto = string.Empty;
        public string ValorRecebidoTexto
        {
            get => _valorRecebidoTexto;
            set
            {
                if (SetProperty(ref _valorRecebidoTexto, value))
                    AtualizarPagamento();
            }
        }

        private string _clienteIdTexto = string.Empty;
        public string ClienteIdTexto
        {
            get => _clienteIdTexto;
            set
            {
                if (SetProperty(ref _clienteIdTexto, value))
                    AtualizarPagamento();
            }
        }

        private string _nomeCliente = string.Empty;
        public string NomeCliente
        {
            get => _nomeCliente;
            set => SetProperty(ref _nomeCliente, value);
        }

        private string _cpfCnpjCliente = string.Empty;
        public string CpfCnpjCliente
        {
            get => _cpfCnpjCliente;
            set => SetProperty(ref _cpfCnpjCliente, value);
        }

        private string _tipoPagamentoSelecionado = "Dinheiro";
        public string TipoPagamentoSelecionado
        {
            get => _tipoPagamentoSelecionado;
            set
            {
                if (SetProperty(ref _tipoPagamentoSelecionado, value))
                    AtualizarPagamento();
            }
        }

        private DateTime? _dataVencimento = DateTime.Today.AddDays(30);
        public DateTime? DataVencimento
        {
            get => _dataVencimento;
            set => SetProperty(ref _dataVencimento, value);
        }

        public decimal TotalVenda => ItensVenda.Sum(i => i.Total);
        public string TotalVendaTexto => TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public int QuantidadeItens => ItensVenda.Count;
        public string TrocoTexto => CalcularTroco().ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public string ValorAReceberTexto => TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        public bool ClienteCadastradoInformado => TryGetClienteCadastradoId(out _);
        public bool MostrarVencimentoFiadoPrazo => EhPagamentoPendente(TipoPagamentoSelecionado) && ClienteCadastradoInformado;

        private bool _mostrarMontagemVenda = true;
        public bool MostrarMontagemVenda
        {
            get => _mostrarMontagemVenda;
            set => SetProperty(ref _mostrarMontagemVenda, value);
        }

        private bool _mostrarPagamento;
        public bool MostrarPagamento
        {
            get => _mostrarPagamento;
            set => SetProperty(ref _mostrarPagamento, value);
        }

        private DateTime? _mesFiltro = DateTime.Today;
        public DateTime? MesFiltro
        {
            get => _mesFiltro;
            set
            {
                SetProperty(ref _mesFiltro, value);
                Carregar();
            }
        }

        private string _statusFiltro = "Todas";
        public string StatusFiltro
        {
            get => _statusFiltro;
            set
            {
                SetProperty(ref _statusFiltro, value);
                Carregar();
            }
        }

        public ICommand AtualizarCommand { get; }
        public ICommand BuscarProdutoCommand { get; }
        public ICommand AdicionarItemCommand { get; }
        public ICommand RemoverItemCommand { get; }
        public ICommand LimparVendaCommand { get; }
        public ICommand ConcluirVendaCommand { get; }
        public ICommand ConfirmarPagamentoCommand { get; }
        public ICommand VoltarPagamentoCommand { get; }

        public VendasViewModel(IPdvOperationService pdvService, IAlertService alertService)
        {
            _pdvService = pdvService;
            _alertService = alertService;
            AtualizarCommand = new RelayCommand(Carregar);
            BuscarProdutoCommand = new RelayCommand(BuscarProdutos);
            AdicionarItemCommand = new RelayCommand(AdicionarItem);
            RemoverItemCommand = new RelayCommand(RemoverItem);
            LimparVendaCommand = new RelayCommand(LimparVenda);
            ConcluirVendaCommand = new RelayCommand(ConcluirVenda);
            ConfirmarPagamentoCommand = new RelayCommand(ConfirmarPagamento);
            VoltarPagamentoCommand = new RelayCommand(VoltarParaMontagem);
            Carregar();
        }

        public VendasViewModel()
        {
            AtualizarCommand = new RelayCommand(() => { });
            BuscarProdutoCommand = new RelayCommand(() => { });
            AdicionarItemCommand = new RelayCommand(() => { });
            RemoverItemCommand = new RelayCommand(() => { });
            LimparVendaCommand = new RelayCommand(() => { });
            ConcluirVendaCommand = new RelayCommand(() => { });
            ConfirmarPagamentoCommand = new RelayCommand(() => { });
            VoltarPagamentoCommand = new RelayCommand(() => { });
        }

        private void Carregar()
        {
            if (_pdvService == null)
                return;

            Vendas.Clear();
            foreach (var venda in _pdvService.ListarVendas(MesFiltro, StatusFiltro))
                Vendas.Add(venda);
        }

        private void BuscarProdutos()
        {
            try
            {
                ProdutosEncontrados.Clear();
                if (_pdvService == null)
                    return;

                foreach (var produto in _pdvService.BuscarProdutos(TermoProduto, 50))
                    ProdutosEncontrados.Add(produto);

                ProdutoSelecionado = ProdutosEncontrados.FirstOrDefault();
                if (ProdutosEncontrados.Count == 0)
                    _alertService?.ShowAlert("Nenhum produto encontrado para a busca informada.", AlertType.Warning);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void AdicionarItem()
        {
            try
            {
                if (ProdutoSelecionado == null)
                {
                    _alertService?.ShowAlert("Selecione um produto para adicionar.", AlertType.Warning);
                    return;
                }

                if (!TryParseValor(QuantidadeTexto, out var quantidade) || quantidade <= 0)
                {
                    _alertService?.ShowAlert("Informe uma quantidade valida.", AlertType.Warning);
                    return;
                }

                var item = new VendaItemLinha(ProdutoSelecionado, quantidade);
                ItensVenda.Add(item);
                ItemSelecionado = item;
                AtualizarTotais();
                FecharPagamentoSeAberto();
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void RemoverItem()
        {
            if (ItemSelecionado == null)
            {
                _alertService?.ShowAlert("Selecione um item para remover.", AlertType.Warning);
                return;
            }

            ItensVenda.Remove(ItemSelecionado);
            ItemSelecionado = ItensVenda.FirstOrDefault();
            AtualizarTotais();
            FecharPagamentoSeAberto();
        }

        private void LimparVenda()
        {
            ItensVenda.Clear();
            ItemSelecionado = null;
            ValorRecebidoTexto = string.Empty;
            VoltarParaMontagem();
            AtualizarTotais();
        }

        private void ConcluirVenda()
        {
            try
            {
                if (ItensVenda.Count == 0)
                {
                    _alertService?.ShowAlert("Adicione ao menos um item antes de concluir a venda.", AlertType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ValorRecebidoTexto))
                    ValorRecebidoTexto = TotalVenda.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));

                MostrarMontagemVenda = false;
                MostrarPagamento = true;
                AtualizarPagamento();
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void ConfirmarPagamento()
        {
            try
            {
                if (_pdvService == null)
                    return;

                if (ItensVenda.Count == 0)
                {
                    _alertService?.ShowAlert("Adicione ao menos um item antes de concluir a venda.", AlertType.Warning);
                    VoltarParaMontagem();
                    return;
                }

                var total = TotalVenda;
                var valorRecebido = total;
                if (!TryParseValor(ValorRecebidoTexto, out valorRecebido))
                {
                    _alertService?.ShowAlert("Informe um valor recebido valido.", AlertType.Warning);
                    return;
                }

                var fiado = EhPagamentoPendente(TipoPagamentoSelecionado);
                var clienteId = TryGetClienteCadastradoId(out var parsedClienteId) ? parsedClienteId : (int?)null;

                if (fiado && !clienteId.HasValue)
                {
                    _alertService?.ShowAlert("Venda a prazo ou fiado exige cliente cadastrado.", AlertType.Warning);
                    return;
                }

                var venda = _pdvService.FinalizarVenda(new VendaInput(
                    ItensVenda.Select(i => new VendaItemDto(i.ProdutoId, i.Gtin, i.Descricao, i.Quantidade, i.ValorUnitario, 0)).ToList(),
                    new[] { new VendaPagamentoDto(TipoPagamentoSelecionado, valorRecebido, fiado ? DataVencimento : null) },
                    clienteId,
                    string.IsNullOrWhiteSpace(NomeCliente) ? null : NomeCliente.Trim(),
                    string.IsNullOrWhiteSpace(CpfCnpjCliente) ? null : CpfCnpjCliente.Trim(),
                    fiado));

                _alertService?.ShowAlert($"Venda #{venda.Id} finalizada com sucesso.", AlertType.Success);
                LimparVenda();
                Carregar();
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

        private void AtualizarTotais()
        {
            OnPropertyChanged(nameof(TotalVenda));
            OnPropertyChanged(nameof(TotalVendaTexto));
            OnPropertyChanged(nameof(QuantidadeItens));
            AtualizarPagamento();
        }

        private void AtualizarPagamento()
        {
            OnPropertyChanged(nameof(ValorAReceberTexto));
            OnPropertyChanged(nameof(TrocoTexto));
            OnPropertyChanged(nameof(ClienteCadastradoInformado));
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

        private bool TryGetClienteCadastradoId(out int clienteId)
        {
            clienteId = 0;
            return !string.IsNullOrWhiteSpace(ClienteIdTexto)
                && int.TryParse(ClienteIdTexto.Trim(), out clienteId)
                && clienteId > 0;
        }
    }

    public class VendaItemLinha
    {
        public VendaItemLinha(ProdutoVendaDto produto, decimal quantidade)
        {
            ProdutoId = produto.Id;
            Gtin = produto.Gtin;
            CodigoInterno = produto.CodigoInterno;
            Descricao = produto.Descricao;
            Quantidade = quantidade;
            ValorUnitario = produto.ValorVenda;
        }

        public int? ProdutoId { get; }
        public string Gtin { get; }
        public string CodigoInterno { get; }
        public string Descricao { get; }
        public decimal Quantidade { get; }
        public decimal ValorUnitario { get; }
        public decimal Total => Quantidade * ValorUnitario;
    }
}

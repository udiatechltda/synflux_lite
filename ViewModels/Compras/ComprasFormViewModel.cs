using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PDV.Commands;
using PDV.Models;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class ComprasFormViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAlertService? _alertService;

        private string _colaborador = string.Empty;
        private string _fornecedor = string.Empty;
        private bool _fornecedorErro;
        private string _nomeContato = string.Empty;

        private DateTime? _dataPedido = DateTime.Today;
        private DateTime? _dataPrevistaEntrega;
        private DateTime? _dataPrevisaoPagamento;
        private string _condicaoPagamento = string.Empty;
        private DateTime? _dataRecebimentoItens;
        private string _horaRecebimentoItens = string.Empty;

        private string _localEntrega = string.Empty;
        private string _localCobranca = string.Empty;
        private string _numeroDocumentoEntrada = string.Empty;
        private decimal _taxaDesconto;

        private decimal _valorSubtotal;
        private decimal _valorDesconto;
        private decimal _valorTotal;

        private string _quantidadeParcelas = string.Empty;
        private string _intervaloParcelas = string.Empty;
        private DateTime? _diaPrimeiroVencimento;
        private string _diaFixoParcela = string.Empty;

        public ComprasFormViewModel(
            IViewModelNavigationService navigationService,
            IPdvOperationService pdvService,
            IAlertService alertService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _alertService = alertService;

            CondicoesPagamento = new ObservableCollection<string>
            {
                "À Vista",
                "30 dias",
                "30/60",
                "30/60/90"
            };

            SalvarCommand = new RelayCommand(Salvar);
            CancelarCommand = new RelayCommand(Cancelar);
            BuscarColaboradorCommand = new RelayCommand(BuscarColaborador);
            BuscarFornecedorCommand = new RelayCommand(BuscarFornecedor);
            IrParaItensCommand = new RelayCommand(IrParaItens);
        }

        // construtor vazio para compatibilidade
        public ComprasFormViewModel()
        {
            CondicoesPagamento = new ObservableCollection<string>
            {
                "À Vista",
                "30 dias",
                "30/60",
                "30/60/90"
            };

            SalvarCommand = new RelayCommand(() => { });
            CancelarCommand = new RelayCommand(() => { });
            BuscarColaboradorCommand = new RelayCommand(() => { });
            BuscarFornecedorCommand = new RelayCommand(() => { });
            IrParaItensCommand = new RelayCommand(() => { });
        }

        public string Colaborador
        {
            get => _colaborador;
            set => SetProperty(ref _colaborador, value);
        }

        public string Fornecedor
        {
            get => _fornecedor;
            set
            {
                SetProperty(ref _fornecedor, value);
                FornecedorErro = string.IsNullOrWhiteSpace(value);
            }
        }

        public bool FornecedorErro
        {
            get => _fornecedorErro;
            set => SetProperty(ref _fornecedorErro, value);
        }

        public string NomeContato
        {
            get => _nomeContato;
            set => SetProperty(ref _nomeContato, value);
        }

        public DateTime? DataPedido
        {
            get => _dataPedido;
            set => SetProperty(ref _dataPedido, value);
        }

        public DateTime? DataPrevistaEntrega
        {
            get => _dataPrevistaEntrega;
            set => SetProperty(ref _dataPrevistaEntrega, value);
        }

        public DateTime? DataPrevisaoPagamento
        {
            get => _dataPrevisaoPagamento;
            set => SetProperty(ref _dataPrevisaoPagamento, value);
        }

        public ObservableCollection<string> CondicoesPagamento { get; }

        public string CondicaoPagamento
        {
            get => _condicaoPagamento;
            set => SetProperty(ref _condicaoPagamento, value);
        }

        public DateTime? DataRecebimentoItens
        {
            get => _dataRecebimentoItens;
            set => SetProperty(ref _dataRecebimentoItens, value);
        }

        public string HoraRecebimentoItens
        {
            get => _horaRecebimentoItens;
            set => SetProperty(ref _horaRecebimentoItens, value);
        }

        public string LocalEntrega
        {
            get => _localEntrega;
            set => SetProperty(ref _localEntrega, value);
        }

        public string LocalCobranca
        {
            get => _localCobranca;
            set => SetProperty(ref _localCobranca, value);
        }

        public string NumeroDocumentoEntrada
        {
            get => _numeroDocumentoEntrada;
            set => SetProperty(ref _numeroDocumentoEntrada, value);
        }

        public decimal TaxaDesconto
        {
            get => _taxaDesconto;
            set
            {
                SetProperty(ref _taxaDesconto, value);
                RecalcularTotais();
            }
        }

        public decimal ValorSubtotal
        {
            get => _valorSubtotal;
            set
            {
                SetProperty(ref _valorSubtotal, value);
                RecalcularTotais();
            }
        }

        public decimal ValorDesconto
        {
            get => _valorDesconto;
            set => SetProperty(ref _valorDesconto, value);
        }

        public decimal ValorTotal
        {
            get => _valorTotal;
            set => SetProperty(ref _valorTotal, value);
        }

        public string QuantidadeParcelas
        {
            get => _quantidadeParcelas;
            set => SetProperty(ref _quantidadeParcelas, value);
        }

        public string IntervaloParcelas
        {
            get => _intervaloParcelas;
            set => SetProperty(ref _intervaloParcelas, value);
        }

        public DateTime? DiaPrimeiroVencimento
        {
            get => _diaPrimeiroVencimento;
            set => SetProperty(ref _diaPrimeiroVencimento, value);
        }

        public string DiaFixoParcela
        {
            get => _diaFixoParcela;
            set => SetProperty(ref _diaFixoParcela, value);
        }

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand BuscarColaboradorCommand { get; }
        public ICommand BuscarFornecedorCommand { get; }
        public ICommand IrParaItensCommand { get; }

        private void RecalcularTotais()
        {
            ValorDesconto = ValorSubtotal * (TaxaDesconto / 100m);
            ValorTotal = ValorSubtotal - ValorDesconto;
        }

        private void Salvar()
        {
            if (string.IsNullOrWhiteSpace(Fornecedor))
            {
                FornecedorErro = true;
                _alertService?.ShowAlert("Fornecedor e obrigatorio para salvar o pedido.", AlertType.Warning);
                return;
            }

            try
            {
                _pdvService?.SalvarPedidoCompra(new CompraPedidoInput(
                    Colaborador,
                    Fornecedor,
                    NomeContato,
                    DataPedido,
                    DataPrevistaEntrega,
                    DataPrevisaoPagamento,
                    CondicaoPagamento,
                    DataRecebimentoItens,
                    HoraRecebimentoItens,
                    LocalEntrega,
                    LocalCobranca,
                    NumeroDocumentoEntrada,
                    ValorSubtotal,
                    TaxaDesconto,
                    ValorDesconto,
                    ValorTotal,
                    ParseInt(QuantidadeParcelas, 1),
                    ParseInt(IntervaloParcelas, 30),
                    DiaPrimeiroVencimento,
                    DiaFixoParcela));

                _alertService?.ShowAlert("Pedido de compra salvo com sucesso.", AlertType.Success);
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
                return;
            }

            _navigationService?.NavigateTo("Compras");
        }

        private void Cancelar()
        {
            _navigationService?.NavigateTo("Compras");
        }

        private void BuscarColaborador() { }

        private void BuscarFornecedor() { }

        private void IrParaItens() { }

        private static int ParseInt(string value, int padrao)
        {
            return int.TryParse(value, out var parsed) ? parsed : padrao;
        }
    }
}

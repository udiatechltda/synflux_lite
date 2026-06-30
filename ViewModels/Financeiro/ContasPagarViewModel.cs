using PDV.Services.Interfaces;
using System.Collections.ObjectModel;

namespace PDV.ViewModels
{
    public class ContasPagarViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;

        public ObservableCollection<ContasPagarItem> Contas { get; } = new();

        private string _filtroAtivo = "Mês/Ano: todos  |  Status: Todos";
        public string FiltroAtivo
        {
            get => _filtroAtivo;
            set { _filtroAtivo = value; OnPropertyChanged(); }
        }

        private decimal _totalPagar;
        public decimal TotalPagar
        {
            get => _totalPagar;
            set { _totalPagar = value; OnPropertyChanged(); }
        }

        private decimal _totalPago;
        public decimal TotalPago
        {
            get => _totalPago;
            set { _totalPago = value; OnPropertyChanged(); }
        }

        private decimal _totalGeral;
        public decimal TotalGeral
        {
            get => _totalGeral;
            set { _totalGeral = value; OnPropertyChanged(); }
        }

        public ContasPagarViewModel() { }

        public ContasPagarViewModel(IViewModelNavigationService navigationService, IPdvOperationService pdvService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            Carregar();
        }

        public void Voltar()
        {
            _navigationService?.NavigateTo("Financeiro");
        }

        private void Carregar()
        {
            if (_pdvService == null)
                return;

            Contas.Clear();
            foreach (var conta in _pdvService.ListarContasPagar())
            {
                Contas.Add(new ContasPagarItem
                {
                    Fornecedor = conta.Fornecedor,
                    Status = conta.Status,
                    DataLancamento = conta.DataLancamento,
                    DataVencimento = conta.DataVencimento,
                    DataPagamento = conta.DataPagamento,
                    ValorAPagar = conta.ValorAPagar,
                    TaxaJuros = conta.TaxaJuros,
                    ValorJuros = conta.ValorJuros,
                    ValorPago = conta.ValorPago
                });
            }

            TotalPagar = Contas.Where(c => !string.Equals(c.Status, "Pago", StringComparison.OrdinalIgnoreCase)).Sum(c => c.ValorAPagar);
            TotalPago = Contas.Sum(c => c.ValorPago);
            TotalGeral = Contas.Sum(c => c.ValorAPagar);
        }
    }

    public class ContasPagarItem
    {
        public string Fornecedor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataLancamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal ValorAPagar { get; set; }
        public decimal TaxaJuros { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorPago { get; set; }
    }
}

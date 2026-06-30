using PDV.Services.Interfaces;
using System.Collections.ObjectModel;

namespace PDV.ViewModels
{
    public class ContasReceberViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;

        public ObservableCollection<ContasReceberItem> Contas { get; } = new();

        private string _filtroAtivo = "Mês/Ano: todos  |  Status: Todos";
        public string FiltroAtivo
        {
            get => _filtroAtivo;
            set { _filtroAtivo = value; OnPropertyChanged(); }
        }

        private decimal _totalReceber;
        public decimal TotalReceber
        {
            get => _totalReceber;
            set { _totalReceber = value; OnPropertyChanged(); }
        }

        private decimal _totalRecebido;
        public decimal TotalRecebido
        {
            get => _totalRecebido;
            set { _totalRecebido = value; OnPropertyChanged(); }
        }

        private decimal _totalGeral;
        public decimal TotalGeral
        {
            get => _totalGeral;
            set { _totalGeral = value; OnPropertyChanged(); }
        }

        public ContasReceberViewModel() { }

        public ContasReceberViewModel(IViewModelNavigationService navigationService, IPdvOperationService pdvService)
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
            foreach (var conta in _pdvService.ListarContasReceber())
            {
                Contas.Add(new ContasReceberItem
                {
                    Cliente = conta.Cliente,
                    Status = conta.Status,
                    DataLancamento = conta.DataLancamento,
                    DataVencimento = conta.DataVencimento,
                    DataRecebimento = conta.DataRecebimento,
                    ValorAReceber = conta.ValorAReceber,
                    TaxaJuros = conta.TaxaJuros,
                    ValorJuros = conta.ValorJuros,
                    ValorRecebido = conta.ValorRecebido
                });
            }

            TotalReceber = Contas.Where(c => !string.Equals(c.Status, "Recebido", StringComparison.OrdinalIgnoreCase)).Sum(c => c.ValorAReceber);
            TotalRecebido = Contas.Sum(c => c.ValorRecebido);
            TotalGeral = Contas.Sum(c => c.ValorAReceber);
        }
    }

    public class ContasReceberItem
    {
        public string Cliente { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataLancamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public decimal ValorAReceber { get; set; }
        public decimal TaxaJuros { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorRecebido { get; set; }
    }
}

using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PDV.Models;
using PDV.Services;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class MovimentoViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly PdvContext? _context;
        private readonly IAlertService? _alertService;

        public ObservableCollection<MovimentoLinhaDto> Linhas { get; } = new();

        private string _tituloMovimento = "Movimento do Caixa";
        public string TituloMovimento
        {
            get => _tituloMovimento;
            set => SetProperty(ref _tituloMovimento, value);
        }

        private string _statusMovimento = "Sem caixa aberto";
        public string StatusMovimento
        {
            get => _statusMovimento;
            set => SetProperty(ref _statusMovimento, value);
        }

        private string _dataAberturaTexto = "-";
        public string DataAberturaTexto
        {
            get => _dataAberturaTexto;
            set => SetProperty(ref _dataAberturaTexto, value);
        }

        private string _horaAbertura = "-";
        public string HoraAbertura
        {
            get => _horaAbertura;
            set => SetProperty(ref _horaAbertura, value);
        }

        private string _totalVendasTexto = "R$ 0,00";
        public string TotalVendasTexto
        {
            get => _totalVendasTexto;
            set => SetProperty(ref _totalVendasTexto, value);
        }

        private string _totalSuprimentosTexto = "R$ 0,00";
        public string TotalSuprimentosTexto
        {
            get => _totalSuprimentosTexto;
            set => SetProperty(ref _totalSuprimentosTexto, value);
        }

        private string _totalSangriasTexto = "R$ 0,00";
        public string TotalSangriasTexto
        {
            get => _totalSangriasTexto;
            set => SetProperty(ref _totalSangriasTexto, value);
        }

        private string _saldoPrevistoTexto = "R$ 0,00";
        public string SaldoPrevistoTexto
        {
            get => _saldoPrevistoTexto;
            set => SetProperty(ref _saldoPrevistoTexto, value);
        }

        private string _totalRegistrosTexto = "0 lancamentos";
        public string TotalRegistrosTexto
        {
            get => _totalRegistrosTexto;
            set => SetProperty(ref _totalRegistrosTexto, value);
        }

        public ICommand VoltarCommand { get; }
        public ICommand AtualizarCommand { get; }

        public MovimentoViewModel(
            IViewModelNavigationService navigationService,
            PdvContext context,
            IAlertService alertService)
        {
            _navigationService = navigationService;
            _context = context;
            _alertService = alertService;

            VoltarCommand = new RelayCommand(() => _navigationService.NavigateTo("Home"));
            AtualizarCommand = new RelayCommand(CarregarMovimento);

            CarregarMovimento();
        }

        public MovimentoViewModel()
        {
            VoltarCommand = new RelayCommand(() => { });
            AtualizarCommand = new RelayCommand(() => { });
        }

        private void CarregarMovimento()
        {
            Linhas.Clear();

            if (_context == null)
                return;

            var movimento = _context.PdvMovimentos
                .AsNoTracking()
                .OrderByDescending(m => m.DataFechamento == null)
                .ThenByDescending(m => m.Id)
                .FirstOrDefault();

            if (movimento == null)
            {
                TituloMovimento = "Movimento do Caixa";
                StatusMovimento = "Nenhum movimento encontrado";
                DataAberturaTexto = "-";
                HoraAbertura = "-";
                AtualizarTotais(0, 0, 0);
                TotalRegistrosTexto = "0 lancamentos";
                _alertService?.ShowAlert("Nenhum movimento de caixa encontrado.", AlertType.Warning);
                return;
            }

            var movimentoId = movimento.Id;
            TituloMovimento = movimento.DataFechamento == null ? "Movimento do Caixa Aberto" : "Ultimo Movimento Fechado";
            StatusMovimento = movimento.DataFechamento == null ? "Aberto" : "Fechado";
            DataAberturaTexto = movimento.DataAbertura?.ToString("dd/MM/yyyy") ?? "-";
            HoraAbertura = movimento.HoraAbertura ?? "-";

            var pagamentosPorVenda = (
                from pagamento in _context.PdvTotaisTipoPagamento.AsNoTracking()
                join tipo in _context.PdvTiposPagamento.AsNoTracking()
                    on pagamento.IdPdvTipoPagamento equals tipo.Id into tipos
                from tipo in tipos.DefaultIfEmpty()
                select new
                {
                    pagamento.IdPdvVendaCabecalho,
                    pagamento.Valor,
                    Tipo = tipo != null ? tipo.Descricao : null
                })
                .ToList()
                .Where(p => p.IdPdvVendaCabecalho.HasValue)
                .GroupBy(p => p.IdPdvVendaCabecalho!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ", g
                        .Select(p => string.IsNullOrWhiteSpace(p.Tipo) ? "Pagamento" : p.Tipo!)
                        .Distinct()));

            var vendas = _context.PdvVendasCabecalho
                .AsNoTracking()
                .Where(v => v.IdPdvMovimento == movimentoId)
                .ToList()
                .Select(v => new MovimentoLinhaDto(
                    v.DataVenda,
                    v.HoraVenda,
                    "Venda",
                    $"Venda #{v.Id}",
                    string.IsNullOrWhiteSpace(v.NomeCliente) ? "Consumidor nao identificado" : v.NomeCliente!,
                    v.Id.HasValue && pagamentosPorVenda.TryGetValue(v.Id.Value, out var forma) ? forma : "-",
                    ToDecimal(v.ValorFinal),
                    "Entrada"));

            var suprimentos = _context.PdvSuprimentos
                .AsNoTracking()
                .Where(s => s.IdPdvMovimento == movimentoId)
                .ToList()
                .Select(s => new MovimentoLinhaDto(
                    s.DataSuprimento,
                    s.HoraSuprimento,
                    "Suprimento",
                    $"Suprimento #{s.Id}",
                    string.IsNullOrWhiteSpace(s.Observacao) ? "Entrada de caixa" : s.Observacao!,
                    "Dinheiro",
                    ToDecimal(s.Valor),
                    "Entrada"));

            var sangrias = _context.PdvSangrias
                .AsNoTracking()
                .Where(s => s.IdPdvMovimento == movimentoId)
                .ToList()
                .Select(s => new MovimentoLinhaDto(
                    s.DataSangria,
                    s.HoraSangria,
                    "Sangria",
                    $"Sangria #{s.Id}",
                    string.IsNullOrWhiteSpace(s.Observacao) ? "Saida de caixa" : s.Observacao!,
                    "Dinheiro",
                    ToDecimal(s.Valor) * -1,
                    "Saida"));

            foreach (var linha in vendas.Concat(suprimentos).Concat(sangrias).OrderBy(l => l.Data).ThenBy(l => l.Hora))
                Linhas.Add(linha);

            var totalVendas = Linhas.Where(l => l.Tipo == "Venda").Sum(l => l.Valor);
            var totalSuprimentos = Linhas.Where(l => l.Tipo == "Suprimento").Sum(l => l.Valor);
            var totalSangrias = Math.Abs(Linhas.Where(l => l.Tipo == "Sangria").Sum(l => l.Valor));

            AtualizarTotais(totalVendas, totalSuprimentos, totalSangrias);
            TotalRegistrosTexto = $"{Linhas.Count} lancamento(s)";
        }

        private void AtualizarTotais(decimal totalVendas, decimal totalSuprimentos, decimal totalSangrias)
        {
            TotalVendasTexto = FormatarMoeda(totalVendas);
            TotalSuprimentosTexto = FormatarMoeda(totalSuprimentos);
            TotalSangriasTexto = FormatarMoeda(totalSangrias);
            SaldoPrevistoTexto = FormatarMoeda(totalVendas + totalSuprimentos - totalSangrias);
        }

        private static decimal ToDecimal(double? value) => Convert.ToDecimal(value ?? 0);

        private static string FormatarMoeda(decimal valor) => valor.ToString("C2");
    }

    public record MovimentoLinhaDto(
        DateTime? Data,
        string? Hora,
        string Tipo,
        string Documento,
        string Descricao,
        string FormaPagamento,
        decimal Valor,
        string Natureza)
    {
        public string DataTexto => Data?.ToString("dd/MM/yyyy") ?? "-";
        public string HoraTexto => string.IsNullOrWhiteSpace(Hora) ? "-" : Hora!;
        public string ValorTexto => Valor.ToString("C2");
    }
}

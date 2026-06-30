using System.Windows.Input;
using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDV.Models;
using PDV.Models.Pdv;
using PDV.Services;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class AberturaCaixaViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAlertService? _alertService;
        private readonly IAuthenticationService? _authenticationService;
        private readonly PdvContext? _context;

        private decimal _fundoTroco;
        public decimal FundoTroco
        {
            get => _fundoTroco;
            set
            {
                if (SetProperty(ref _fundoTroco, value))
                    OnPropertyChanged(nameof(FundoTrocoFormatado));
            }
        }

        private string _fundoTrocoTexto = string.Empty;
        public string FundoTrocoTexto
        {
            get => _fundoTrocoTexto;
            set => SetProperty(ref _fundoTrocoTexto, value);
        }

        public string FundoTrocoFormatado => FundoTroco.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));

        private DateTime _dataAbertura = DateTime.Today;
        public DateTime DataAbertura
        {
            get => _dataAbertura;
            private set => SetProperty(ref _dataAbertura, value);
        }

        private string _horaAbertura = DateTime.Now.ToString("HH:mm:ss");
        public string HoraAbertura
        {
            get => _horaAbertura;
            private set => SetProperty(ref _horaAbertura, value);
        }

        private bool _isCaixaAberto;
        public bool IsCaixaAberto
        {
            get => _isCaixaAberto;
            private set
            {
                if (!SetProperty(ref _isCaixaAberto, value))
                    return;

                OnPropertyChanged(nameof(PodeInformarFundo));
                OnPropertyChanged(nameof(TextoAcaoCaixa));
                OnPropertyChanged(nameof(TituloTela));
                OnPropertyChanged(nameof(DescricaoTela));
                OnPropertyChanged(nameof(TextoTotalInicial));
            }
        }

        public bool PodeInformarFundo => !IsCaixaAberto;
        public string TextoAcaoCaixa => IsCaixaAberto ? "Confirmar Fechamento" : "Abrir Caixa";
        public string TituloTela => IsCaixaAberto ? "Fechamento de Caixa" : "Abertura de Caixa";
        public string DescricaoTela => IsCaixaAberto
            ? "Confira os totais por forma de pagamento antes de fechar o caixa."
            : "Informe o fundo de troco para iniciar a operacao.";
        public string TextoTotalInicial => IsCaixaAberto ? "Total do caixa:" : "Total inicial informado:";

        public ObservableCollection<FechamentoResumoLinha> TotaisPagamento { get; } = new();

        private string _totalCaixaTexto = "R$ 0,00";
        public string TotalCaixaTexto
        {
            get => _totalCaixaTexto;
            private set => SetProperty(ref _totalCaixaTexto, value);
        }

        private string _totalDinheiroTexto = "R$ 0,00";
        public string TotalDinheiroTexto
        {
            get => _totalDinheiroTexto;
            private set => SetProperty(ref _totalDinheiroTexto, value);
        }

        private string _totalDebitoTexto = "R$ 0,00";
        public string TotalDebitoTexto
        {
            get => _totalDebitoTexto;
            private set => SetProperty(ref _totalDebitoTexto, value);
        }

        private string _totalCreditoTexto = "R$ 0,00";
        public string TotalCreditoTexto
        {
            get => _totalCreditoTexto;
            private set => SetProperty(ref _totalCreditoTexto, value);
        }

        private string _totalPixTexto = "R$ 0,00";
        public string TotalPixTexto
        {
            get => _totalPixTexto;
            private set => SetProperty(ref _totalPixTexto, value);
        }

        private string _totalOutrosTexto = "R$ 0,00";
        public string TotalOutrosTexto
        {
            get => _totalOutrosTexto;
            private set => SetProperty(ref _totalOutrosTexto, value);
        }

        private string _totalVendasTexto = "R$ 0,00";
        public string TotalVendasTexto
        {
            get => _totalVendasTexto;
            private set => SetProperty(ref _totalVendasTexto, value);
        }

        private string _totalSuprimentosTexto = "R$ 0,00";
        public string TotalSuprimentosTexto
        {
            get => _totalSuprimentosTexto;
            private set => SetProperty(ref _totalSuprimentosTexto, value);
        }

        private string _totalSangriasTexto = "R$ 0,00";
        public string TotalSangriasTexto
        {
            get => _totalSangriasTexto;
            private set => SetProperty(ref _totalSangriasTexto, value);
        }

        public ICommand Somar50Command { get; }
        public ICommand Somar100Command { get; }
        public ICommand Somar200Command { get; }
        public ICommand LimparCommand { get; }
        public ICommand ConfirmarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AberturaCaixaViewModel(
            IViewModelNavigationService navigationService,
            IPdvOperationService pdvService,
            IAlertService alertService,
            IAuthenticationService authenticationService,
            PdvContext context)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _alertService = alertService;
            _authenticationService = authenticationService;
            _context = context;

            Somar50Command = new RelayCommand(() => SomarFundo(50));
            Somar100Command = new RelayCommand(() => SomarFundo(100));
            Somar200Command = new RelayCommand(() => SomarFundo(200));
            LimparCommand = new RelayCommand(LimparFundo);
            ConfirmarCommand = new RelayCommand(Confirmar);
            CancelarCommand = new RelayCommand(Cancelar);
            AtualizarEstadoCaixa();
        }

        public AberturaCaixaViewModel()
        {
            Somar50Command = new RelayCommand(() => { });
            Somar100Command = new RelayCommand(() => { });
            Somar200Command = new RelayCommand(() => { });
            LimparCommand = new RelayCommand(() => { });
            ConfirmarCommand = new RelayCommand(() => { });
            CancelarCommand = new RelayCommand(() => { });
        }

        private void Confirmar()
        {
            try
            {
                if (IsCaixaAberto)
                {
                    FecharCaixa();
                    return;
                }

                if (!TryParseValor(FundoTrocoTexto, out var fundoTroco))
                {
                    _alertService?.ShowAlert("Informe um fundo de troco valido.", AlertType.Warning);
                    return;
                }

                if (fundoTroco < 0)
                {
                    _alertService?.ShowAlert("Fundo de troco nao pode ser negativo.", AlertType.Warning);
                    return;
                }

                FundoTroco = fundoTroco;
                var movimento = _pdvService?.AbrirMovimento(FundoTroco);
                _alertService?.ShowAlert($"Caixa aberto com sucesso. Movimento #{movimento?.Id}.", AlertType.Success);
                AtualizarEstadoCaixa();
                _navigationService?.NavigateTo("Home");
            }
            catch (Exception ex)
            {
                _alertService?.ShowAlert(ex.Message, AlertType.Error);
            }
        }

        private void AtualizarEstadoCaixa()
        {
            var movimento = _pdvService?.ObterMovimentoAberto();
            IsCaixaAberto = movimento != null;

            if (movimento == null)
            {
                LimparResumoFechamento();
                return;
            }

            DataAbertura = movimento.DataAbertura ?? DateTime.Today;
            HoraAbertura = movimento.HoraAbertura ?? DateTime.Now.ToString("HH:mm:ss");
            FundoTroco = Convert.ToDecimal(movimento.TotalSuprimento ?? 0);
            FundoTrocoTexto = FundoTrocoFormatado;
            CarregarResumoFechamento(movimento);
        }

        private void Cancelar()
        {
            if (IsCaixaAberto)
            {
                _navigationService?.NavigateTo("Home");
                return;
            }

            _navigationService?.NavigateTo(PdvAccessPolicy.IsOperador(_authenticationService) ? "AberturaCaixa" : "Dashboard");
        }

        private void FecharCaixa()
        {
            var totalCaixa = ParseMoeda(TotalCaixaTexto);
            var movimento = _pdvService?.FecharMovimento(totalCaixa, null);
            _alertService?.ShowAlert($"Caixa fechado com sucesso. Movimento #{movimento?.Id}.", AlertType.Success);
            AtualizarEstadoCaixa();
            _navigationService?.NavigateTo("Dashboard");
        }

        private void CarregarResumoFechamento(PdvMovimento movimento)
        {
            LimparResumoFechamento();
            if (_context == null || movimento.Id == null)
                return;

            var movimentoId = movimento.Id.Value;
            var totalVendas = ToDecimal(_context.PdvVendasCabecalho
                .AsNoTracking()
                .Where(v => v.IdPdvMovimento == movimentoId && (v.StatusVenda == null || v.StatusVenda != "C"))
                .Sum(v => v.ValorFinal ?? 0));

            var totalSuprimentos = ToDecimal(_context.PdvSuprimentos
                .AsNoTracking()
                .Where(s => s.IdPdvMovimento == movimentoId)
                .Sum(s => s.Valor ?? 0));

            var totalSangrias = ToDecimal(_context.PdvSangrias
                .AsNoTracking()
                .Where(s => s.IdPdvMovimento == movimentoId)
                .Sum(s => s.Valor ?? 0));

            var pagamentos = (
                from pagamento in _context.PdvTotaisTipoPagamento.AsNoTracking()
                join venda in _context.PdvVendasCabecalho.AsNoTracking()
                    on pagamento.IdPdvVendaCabecalho equals venda.Id
                join tipo in _context.PdvTiposPagamento.AsNoTracking()
                    on pagamento.IdPdvTipoPagamento equals tipo.Id into tipos
                from tipo in tipos.DefaultIfEmpty()
                where venda.IdPdvMovimento == movimentoId && (venda.StatusVenda == null || venda.StatusVenda != "C")
                select new
                {
                    Tipo = tipo.Descricao ?? "Outros",
                    Valor = pagamento.Valor ?? 0
                })
                .ToList()
                .Select(p =>
                {
                    var normalizado = NormalizarTipoPagamento(p.Tipo);
                    return normalizado with { Valor = ToDecimal(p.Valor) };
                })
                .GroupBy(p => new { p.FormaPagamento, p.Categoria, p.Ordem })
                .Select(g => new FechamentoResumoLinha(g.Key.FormaPagamento, g.Sum(x => x.Valor), g.Key.Categoria, g.Key.Ordem))
                .OrderBy(l => l.Ordem)
                .ThenBy(l => l.FormaPagamento)
                .ToList();

            var dinheiroVenda = pagamentos.Where(p => p.Categoria == CategoriaPagamento.Dinheiro).Sum(p => p.Valor);
            var debito = pagamentos.Where(p => p.Categoria == CategoriaPagamento.Debito).Sum(p => p.Valor);
            var credito = pagamentos.Where(p => p.Categoria == CategoriaPagamento.Credito).Sum(p => p.Valor);
            var pix = pagamentos.Where(p => p.Categoria == CategoriaPagamento.Pix).Sum(p => p.Valor);
            var outros = pagamentos.Where(p => p.Categoria == CategoriaPagamento.Outros).Sum(p => p.Valor);
            var dinheiroCaixa = dinheiroVenda + totalSuprimentos - totalSangrias;
            var totalCaixa = totalVendas + totalSuprimentos - totalSangrias;

            TotalVendasTexto = FormatarMoeda(totalVendas);
            TotalSuprimentosTexto = FormatarMoeda(totalSuprimentos);
            TotalSangriasTexto = FormatarMoeda(totalSangrias);
            TotalDinheiroTexto = FormatarMoeda(dinheiroCaixa);
            TotalDebitoTexto = FormatarMoeda(debito);
            TotalCreditoTexto = FormatarMoeda(credito);
            TotalPixTexto = FormatarMoeda(pix);
            TotalOutrosTexto = FormatarMoeda(outros);
            TotalCaixaTexto = FormatarMoeda(totalCaixa);

            TotaisPagamento.Add(new FechamentoResumoLinha("Dinheiro em caixa", dinheiroCaixa, CategoriaPagamento.Dinheiro, 1));
            TotaisPagamento.Add(new FechamentoResumoLinha("Debito", debito, CategoriaPagamento.Debito, 2));
            TotaisPagamento.Add(new FechamentoResumoLinha("Credito", credito, CategoriaPagamento.Credito, 3));
            TotaisPagamento.Add(new FechamentoResumoLinha("Pix", pix, CategoriaPagamento.Pix, 4));

            if (outros > 0)
                TotaisPagamento.Add(new FechamentoResumoLinha("Outros", outros, CategoriaPagamento.Outros, 5));
        }

        private void LimparResumoFechamento()
        {
            TotaisPagamento.Clear();
            TotalCaixaTexto = FormatarMoeda(0);
            TotalDinheiroTexto = FormatarMoeda(0);
            TotalDebitoTexto = FormatarMoeda(0);
            TotalCreditoTexto = FormatarMoeda(0);
            TotalPixTexto = FormatarMoeda(0);
            TotalOutrosTexto = FormatarMoeda(0);
            TotalVendasTexto = FormatarMoeda(0);
            TotalSuprimentosTexto = FormatarMoeda(0);
            TotalSangriasTexto = FormatarMoeda(0);
        }

        private void SomarFundo(decimal valor)
        {
            var atual = TryParseValor(FundoTrocoTexto, out var parsed) ? parsed : 0;
            FundoTroco = atual + valor;
            FundoTrocoTexto = FundoTrocoFormatado;
        }

        private void LimparFundo()
        {
            FundoTroco = 0;
            FundoTrocoTexto = string.Empty;
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

        private static decimal ToDecimal(double valor) => Convert.ToDecimal(valor);
        private static string FormatarMoeda(decimal valor) => valor.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));

        private static decimal ParseMoeda(string texto)
        {
            texto = (texto ?? string.Empty).Replace("R$", string.Empty).Trim();
            return TryParseValor(texto, out var valor) ? valor : 0;
        }

        private static FechamentoResumoLinha NormalizarTipoPagamento(string tipo)
        {
            tipo = string.IsNullOrWhiteSpace(tipo) ? "Outros" : tipo.Trim();
            if (tipo.Contains("Dinheiro", StringComparison.OrdinalIgnoreCase))
                return new FechamentoResumoLinha("Dinheiro", 0, CategoriaPagamento.Dinheiro, 1);
            if (tipo.Contains("Debito", StringComparison.OrdinalIgnoreCase) || tipo.Contains("Débito", StringComparison.OrdinalIgnoreCase))
                return new FechamentoResumoLinha("Debito", 0, CategoriaPagamento.Debito, 2);
            if (tipo.Contains("Credito", StringComparison.OrdinalIgnoreCase) || tipo.Contains("Crédito", StringComparison.OrdinalIgnoreCase))
                return new FechamentoResumoLinha("Credito", 0, CategoriaPagamento.Credito, 3);
            if (tipo.Contains("Pix", StringComparison.OrdinalIgnoreCase))
                return new FechamentoResumoLinha("Pix", 0, CategoriaPagamento.Pix, 4);

            return new FechamentoResumoLinha(tipo, 0, CategoriaPagamento.Outros, 5);
        }
    }

    public enum CategoriaPagamento
    {
        Dinheiro,
        Debito,
        Credito,
        Pix,
        Outros
    }

    public record FechamentoResumoLinha(
        string FormaPagamento,
        decimal Valor,
        CategoriaPagamento Categoria = CategoriaPagamento.Outros,
        int Ordem = 99)
    {
        public string ValorTexto => Valor.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
    }
}

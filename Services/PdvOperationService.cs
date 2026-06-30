using Microsoft.EntityFrameworkCore;
using PDV.Models.Pdv;
using PDV.Models.Pdv.Cadastros;
using PDV.Models.Pdv.Financeiro;
using PDV.Services.Interfaces;
using System.IO;

namespace PDV.Services
{
    public class PdvOperationService : IPdvOperationService
    {
        private readonly PdvContext _context;
        private readonly IRetaguardaSyncService _syncService;
        private readonly IFiscalNfceService? _fiscalNfceService;
        private readonly ILocalDatabaseService _databaseService;
        private readonly IProdutoImagemService _produtoImagemService;
        private readonly PdvCashSessionState _cashSessionState;

        public PdvOperationService(
            PdvContext context,
            IRetaguardaSyncService syncService,
            ILocalDatabaseService databaseService,
            PdvCashSessionState cashSessionState,
            IProdutoImagemService produtoImagemService,
            IFiscalNfceService? fiscalNfceService = null)
        {
            _context = context;
            _syncService = syncService;
            _databaseService = databaseService;
            _cashSessionState = cashSessionState;
            _produtoImagemService = produtoImagemService;
            _fiscalNfceService = fiscalNfceService;
        }

        public PdvMovimento? ObterMovimentoAberto()
        {
            var movimento = ObterMovimentoPersistidoAberto();
            return movimento != null && _cashSessionState.IsOpen(DatabasePath, movimento.Id)
                ? movimento
                : null;
        }

        private PdvMovimento? ObterMovimentoPersistidoAberto()
        {
            return _context.PdvMovimentos
                .Where(m => m.DataFechamento == null && (m.StatusMovimento == null || m.StatusMovimento != "F"))
                .OrderByDescending(m => m.Id)
                .FirstOrDefault();
        }

        private string DatabasePath => Path.GetFullPath(_databaseService.CurrentDatabasePath);

        public bool ExisteMovimentoAberto() => ObterMovimentoAberto() != null;

        public PdvMovimento AbrirMovimento(decimal fundoTroco)
        {
            if (fundoTroco < 0)
                throw new InvalidOperationException("Fundo de troco nao pode ser negativo.");

            if (fundoTroco > 100000)
                throw new InvalidOperationException("Fundo de troco acima do limite permitido.");

            var aberto = ObterMovimentoPersistidoAberto();
            if (aberto != null)
            {
                _cashSessionState.MarkOpen(DatabasePath, aberto.Id);
                return aberto;
            }

            using var transaction = _context.Database.BeginTransaction();
            var movimento = CriarMovimentoAberto(fundoTroco);
            _context.PdvMovimentos.Add(movimento);
            _context.SaveChanges();

            if (fundoTroco > 0)
            {
                var agora = DateTime.Now;
                _context.PdvSuprimentos.Add(new PdvSuprimento
                {
                    Id = null,
                    IdPdvMovimento = movimento.Id,
                    DataSuprimento = agora.Date,
                    HoraSuprimento = agora.ToString("HH:mm:ss"),
                    Valor = ToDouble(fundoTroco),
                    Observacao = "Fundo de troco inicial"
                });
                _context.SaveChanges();
            }

            transaction.Commit();
            _cashSessionState.MarkOpen(DatabasePath, movimento.Id);
            SincronizarRetaguardaSeAutenticado();
            return movimento;
        }

        private static PdvMovimento CriarMovimentoAberto(decimal fundoTroco)
        {
            var agora = DateTime.Now;
            return new PdvMovimento
            {
                Id = null,
                DataAbertura = agora.Date,
                HoraAbertura = agora.ToString("HH:mm:ss"),
                StatusMovimento = "A",
                TotalSuprimento = ToDouble(fundoTroco),
                TotalSangria = 0,
                TotalVenda = 0,
                TotalDesconto = 0,
                TotalAcrescimo = 0,
                TotalFinal = 0,
                TotalRecebido = 0,
                TotalTroco = 0,
                TotalCancelado = 0
            };
        }

        public IReadOnlyList<ProdutoVendaDto> BuscarProdutos(string termo, int limite = 20)
        {
            termo = (termo ?? string.Empty).Trim();
            if (termo.Length < 2)
                return Array.Empty<ProdutoVendaDto>();

            var like = $"%{termo}%";

            var produtos = _context.Produtos
                .AsNoTracking()
                .Where(p =>
                    EF.Functions.Like(p.Gtin ?? string.Empty, like) ||
                    EF.Functions.Like(p.CodigoInterno ?? string.Empty, like) ||
                    EF.Functions.Like(p.Nome ?? string.Empty, like) ||
                    EF.Functions.Like(p.Descricao ?? string.Empty, like) ||
                    EF.Functions.Like(p.DescricaoPdv ?? string.Empty, like))
                .OrderBy(p => p.Nome)
                .Take(limite)
                .Select(p => new
                {
                    p.Id,
                    Gtin = p.Gtin ?? string.Empty,
                    CodigoInterno = p.CodigoInterno ?? string.Empty,
                    Descricao = p.DescricaoPdv ?? p.Nome ?? p.Descricao ?? "Produto sem descricao",
                    ValorVenda = ToDecimal(p.ValorVenda),
                    QuantidadeEstoque = ToDecimal(p.QuantidadeEstoque)
                })
                .ToList();

            var produtoIds = produtos.Select(p => p.Id).ToList();
            var imagens = _produtoImagemService.ObterImagensLocalPath(produtoIds);

            return produtos
                .Select(p => new ProdutoVendaDto(
                    p.Id,
                    p.Gtin,
                    p.CodigoInterno,
                    p.Descricao,
                    p.ValorVenda,
                    p.QuantidadeEstoque,
                    p.Id.HasValue && imagens.TryGetValue(p.Id.Value, out var imagemPath) ? imagemPath : null))
                .ToList();
        }

        public VendaResumoDto FinalizarVenda(IEnumerable<VendaItemDto> itens, decimal valorRecebido)
        {
            return FinalizarVenda(new VendaInput(
                itens,
                new[] { new VendaPagamentoDto("Dinheiro", valorRecebido) }));
        }

        public VendaResumoDto FinalizarVenda(VendaInput input)
        {
            var itensVenda = input.Itens.Where(i => i.Quantidade > 0).ToList();
            if (itensVenda.Count == 0)
                throw new InvalidOperationException("Informe ao menos um item para finalizar a venda.");

            var total = itensVenda.Sum(i => (i.Quantidade * i.ValorUnitario) - i.Desconto);
            var pagamentos = input.Pagamentos.Where(p => p.Valor > 0).ToList();
            if (pagamentos.Count == 0)
                throw new InvalidOperationException("Informe ao menos uma forma de pagamento.");

            var valorRecebido = pagamentos.Sum(p => p.Valor);
            if (valorRecebido < total)
                throw new InvalidOperationException("Valor recebido menor que o total da venda.");

            using var transaction = _context.Database.BeginTransaction();

            var movimento = ObterMovimentoAberto()
                ?? throw new InvalidOperationException("Abra o caixa antes de iniciar uma venda.");
            var agora = DateTime.Now;
            var troco = valorRecebido - total;
            var desconto = itensVenda.Sum(i => i.Desconto);
            var cliente = input.ClienteId.HasValue
                ? _context.Clientes.AsNoTracking().FirstOrDefault(c => c.Id == input.ClienteId)
                : null;

            var venda = new PdvVendaCabecalho
            {
                Id = null,
                IdCliente = input.ClienteId,
                IdPdvMovimento = movimento.Id,
                DataVenda = agora.Date,
                HoraVenda = agora.ToString("HH:mm:ss"),
                ValorVenda = ToDouble(total + desconto),
                TaxaDesconto = 0,
                ValorDesconto = ToDouble(desconto),
                TaxaAcrescimo = 0,
                ValorAcrescimo = 0,
                ValorFinal = ToDouble(total),
                ValorRecebido = ToDouble(valorRecebido),
                ValorTroco = ToDouble(troco),
                ValorTotalProdutos = ToDouble(total + desconto),
                ValorTotalDocumento = ToDouble(total),
                StatusVenda = "F",
                TipoOperacao = input.ClienteFiado ? "VENDA_FIADO" : "VENDA",
                NomeCliente = input.NomeCliente ?? cliente?.Nome ?? cliente?.Fantasia,
                CpfCnpjCliente = input.CpfCnpjCliente ?? cliente?.CpfCnpj
            };

            _context.PdvVendasCabecalho.Add(venda);
            _context.SaveChanges();

            var itemNumero = 1;
            foreach (var item in itensVenda)
            {
                var detalhe = new PdvVendaDetalhe
                {
                    Id = null,
                    IdPdvVendaCabecalho = venda.Id,
                    IdProduto = item.ProdutoId,
                    Gtin = item.Gtin,
                    Item = itemNumero++,
                    Quantidade = ToDouble(item.Quantidade),
                    ValorUnitario = ToDouble(item.ValorUnitario),
                    ValorTotal = ToDouble(item.Quantidade * item.ValorUnitario),
                    ValorTotalItem = ToDouble((item.Quantidade * item.ValorUnitario) - item.Desconto),
                    TaxaDesconto = 0,
                    ValorDesconto = ToDouble(item.Desconto),
                    Cancelado = "N",
                    MovimentaEstoque = "S"
                };

                _context.PdvVendasDetalhe.Add(detalhe);

                if (item.ProdutoId.HasValue)
                {
                    var produto = _context.Produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                    if (produto != null)
                        produto.QuantidadeEstoque = (produto.QuantidadeEstoque ?? 0) - ToDouble(item.Quantidade);
                }
            }

            movimento.TotalVenda = (movimento.TotalVenda ?? 0) + ToDouble(total + desconto);
            movimento.TotalDesconto = (movimento.TotalDesconto ?? 0) + ToDouble(desconto);
            movimento.TotalFinal = (movimento.TotalFinal ?? 0) + ToDouble(total);
            movimento.TotalRecebido = (movimento.TotalRecebido ?? 0) + ToDouble(valorRecebido);
            movimento.TotalTroco = (movimento.TotalTroco ?? 0) + ToDouble(troco);

            foreach (var pagamento in pagamentos)
            {
                var tipoPagamentoId = ObterOuCriarTipoPagamentoId(pagamento.TipoPagamento);
                _context.PdvTotaisTipoPagamento.Add(new PdvTotalTipoPagamento
                {
                    Id = null,
                    IdPdvVendaCabecalho = venda.Id,
                    IdPdvTipoPagamento = tipoPagamentoId,
                    DataVenda = agora.Date,
                    HoraVenda = agora.ToString("HH:mm:ss"),
                    Valor = ToDouble(pagamento.Valor),
                    Nsu = pagamento.Nsu,
                    Rede = pagamento.Rede,
                    Estorno = "N",
                    CartaoDc = ResolverCartaoDc(pagamento.TipoPagamento)
                });
            }

            var vendaFiado = input.ClienteFiado || pagamentos.Any(p => EhPagamentoPendente(p.TipoPagamento));
            _context.ContasReceber.Add(new ContasReceber
            {
                Id = null,
                IdCliente = input.ClienteId,
                IdPdvVendaCabecalho = venda.Id,
                DataLancamento = agora.Date,
                DataVencimento = pagamentos.FirstOrDefault(p => p.DataVencimento.HasValue)?.DataVencimento ?? (vendaFiado ? agora.Date.AddDays(30) : agora.Date),
                DataRecebimento = vendaFiado ? null : agora.Date,
                ValorAReceber = ToDouble(total),
                ValorRecebido = vendaFiado ? 0 : ToDouble(total),
                TaxaJuro = 0,
                ValorJuro = 0,
                NumeroDocumento = venda.Id?.ToString(),
                Historico = vendaFiado ? $"Venda PDV fiado #{venda.Id}" : $"Venda PDV #{venda.Id}",
                StatusRecebimento = vendaFiado ? "A Receber" : "Recebido"
            });

            if (vendaFiado)
            {
                _context.ClientesFiados.Add(new ClienteFiado
                {
                    Id = null,
                    IdCliente = input.ClienteId,
                    IdPdvVendaCabecalho = venda.Id,
                    ValorPendente = ToDouble(total),
                    DataLancamento = agora.Date,
                    DataPagamento = pagamentos.FirstOrDefault(p => p.DataVencimento.HasValue)?.DataVencimento ?? agora.Date.AddDays(30)
                });
            }

            _context.SaveChanges();
            transaction.Commit();
            EmitirNfceSeConfigurado(venda.Id);
            SincronizarRetaguardaSeAutenticado();

            return MapVenda(venda);
        }

        public void RegistrarSangria(decimal valor, string? observacao)
        {
            if (valor <= 0)
                throw new InvalidOperationException("Informe um valor maior que zero para a sangria.");

            using var transaction = _context.Database.BeginTransaction();
            var movimento = ObterMovimentoAberto()
                ?? throw new InvalidOperationException("Abra o caixa antes de registrar sangria.");
            var agora = DateTime.Now;

            _context.PdvSangrias.Add(new PdvSangria
            {
                Id = null,
                IdPdvMovimento = movimento.Id,
                DataSangria = agora.Date,
                HoraSangria = agora.ToString("HH:mm:ss"),
                Valor = ToDouble(valor),
                Observacao = observacao
            });

            movimento.TotalSangria = (movimento.TotalSangria ?? 0) + ToDouble(valor);
            _context.SaveChanges();
            transaction.Commit();
            SincronizarRetaguardaSeAutenticado();
        }

        public void RegistrarSuprimento(decimal valor, string? observacao)
        {
            if (valor <= 0)
                throw new InvalidOperationException("Informe um valor maior que zero para o suprimento.");

            using var transaction = _context.Database.BeginTransaction();
            var movimento = ObterMovimentoAberto()
                ?? throw new InvalidOperationException("Abra o caixa antes de registrar suprimento.");
            var agora = DateTime.Now;

            _context.PdvSuprimentos.Add(new PdvSuprimento
            {
                Id = null,
                IdPdvMovimento = movimento.Id,
                DataSuprimento = agora.Date,
                HoraSuprimento = agora.ToString("HH:mm:ss"),
                Valor = ToDouble(valor),
                Observacao = observacao
            });

            movimento.TotalSuprimento = (movimento.TotalSuprimento ?? 0) + ToDouble(valor);
            _context.SaveChanges();
            transaction.Commit();
            SincronizarRetaguardaSeAutenticado();
        }

        public PdvMovimento FecharMovimento(decimal valorInformado, string? tipoPagamento)
        {
            using var transaction = _context.Database.BeginTransaction();

            var movimento = ObterMovimentoAberto();

            if (movimento == null)
                throw new InvalidOperationException("Nao existe movimento aberto para fechamento.");

            var agora = DateTime.Now;
            movimento.DataFechamento = agora.Date;
            movimento.HoraFechamento = agora.ToString("HH:mm:ss");
            movimento.StatusMovimento = "F";

            var tipoPagamentoId = ResolverTipoPagamentoId(tipoPagamento);
            _context.PdvFechamentos.Add(new PdvFechamento
            {
                Id = null,
                IdPdvMovimento = movimento.Id,
                IdPdvTipoPagamento = tipoPagamentoId,
                Valor = ToDouble(valorInformado)
            });

            _context.SaveChanges();
            transaction.Commit();
            _cashSessionState.MarkClosed(DatabasePath, movimento.Id);
            SincronizarRetaguardaSeAutenticado();

            return movimento;
        }

        public IReadOnlyList<VendaResumoDto> ListarVendas(DateTime? mes = null, string? status = null)
        {
            var query = _context.PdvVendasCabecalho.AsNoTracking().AsQueryable();

            if (mes.HasValue)
            {
                var inicio = new DateTime(mes.Value.Year, mes.Value.Month, 1);
                var fim = inicio.AddMonths(1);
                query = query.Where(v => v.DataVenda >= inicio && v.DataVenda < fim);
            }

            status = (status ?? "Todas").Trim();
            if (status.Equals("Fechadas", StringComparison.OrdinalIgnoreCase))
                query = query.Where(v => v.StatusVenda == "F");
            else if (status.Equals("Abertas", StringComparison.OrdinalIgnoreCase))
                query = query.Where(v => v.StatusVenda == "A");
            else if (status.Equals("Canceladas", StringComparison.OrdinalIgnoreCase))
                query = query.Where(v => v.StatusVenda == "C");

            return query
                .OrderByDescending(v => v.DataVenda)
                .ThenByDescending(v => v.HoraVenda)
                .Take(500)
                .AsEnumerable()
                .Select(MapVenda)
                .ToList();
        }

        public IReadOnlyList<EstoqueItemDto> ListarEstoque(string? status = null)
        {
            var unidades = _context.ProdutosUnidades.AsNoTracking()
                .Where(u => u.Id.HasValue)
                .ToDictionary(u => u.Id!.Value, u => u.Sigla ?? u.Descricao ?? string.Empty);

            var produtos = _context.Produtos.AsNoTracking()
                .OrderBy(p => p.Nome)
                .Take(1000)
                .AsEnumerable()
                .Select(p =>
                {
                    var estoque = ToDecimal(p.QuantidadeEstoque);
                    var minimo = ToDecimal(p.EstoqueMinimo);
                    var critico = estoque <= minimo;

                    return new EstoqueItemDto(
                        p.Id,
                        estoque,
                        minimo,
                        ToDecimal(p.EstoqueMaximo),
                        p.Nome ?? p.DescricaoPdv ?? p.Descricao ?? "Produto sem nome",
                        p.IdProdutoUnidade.HasValue && unidades.TryGetValue(p.IdProdutoUnidade.Value, out var unidade) ? unidade : string.Empty,
                        p.Gtin ?? string.Empty,
                        p.CodigoInterno ?? string.Empty,
                        p.Descricao ?? p.DescricaoPdv ?? string.Empty,
                        critico);
                });

            if ((status ?? string.Empty).Equals("Critico", StringComparison.OrdinalIgnoreCase) ||
                (status ?? string.Empty).Equals("Crítico", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.Critico);
            }

            return produtos.ToList();
        }

        public IReadOnlyList<ContaPagarDto> ListarContasPagar()
        {
            var fornecedores = _context.Fornecedores.AsNoTracking()
                .Where(f => f.Id.HasValue)
                .ToDictionary(f => f.Id!.Value, f => f.Nome ?? f.Fantasia ?? $"Fornecedor {f.Id}");

            return _context.ContasPagar.AsNoTracking()
                .OrderByDescending(c => c.DataLancamento)
                .Take(500)
                .AsEnumerable()
                .Select(c => new ContaPagarDto(
                    c.IdFornecedor.HasValue && fornecedores.TryGetValue(c.IdFornecedor.Value, out var fornecedor) ? fornecedor : "Fornecedor nao informado",
                    c.StatusPagamento ?? "A Pagar",
                    c.DataLancamento ?? DateTime.MinValue,
                    c.DataVencimento ?? DateTime.MinValue,
                    c.DataPagamento,
                    ToDecimal(c.ValorAPagar),
                    ToDecimal(c.TaxaJuro),
                    ToDecimal(c.ValorJuro),
                    ToDecimal(c.ValorPago)))
                .ToList();
        }

        public IReadOnlyList<ContaReceberDto> ListarContasReceber()
        {
            var clientes = _context.Clientes.AsNoTracking()
                .Where(c => c.Id.HasValue)
                .ToDictionary(c => c.Id!.Value, c => c.Nome ?? c.Fantasia ?? $"Cliente {c.Id}");

            return _context.ContasReceber.AsNoTracking()
                .OrderByDescending(c => c.DataLancamento)
                .Take(500)
                .AsEnumerable()
                .Select(c => new ContaReceberDto(
                    c.IdCliente.HasValue && clientes.TryGetValue(c.IdCliente.Value, out var cliente) ? cliente : "Consumidor",
                    c.StatusRecebimento ?? "A Receber",
                    c.DataLancamento ?? DateTime.MinValue,
                    c.DataVencimento ?? DateTime.MinValue,
                    c.DataRecebimento,
                    ToDecimal(c.ValorAReceber),
                    ToDecimal(c.TaxaJuro),
                    ToDecimal(c.ValorJuro),
                    ToDecimal(c.ValorRecebido)))
                .ToList();
        }

        public IReadOnlyList<CompraPedidoDto> ListarCompras()
        {
            var fornecedores = _context.Fornecedores.AsNoTracking()
                .Where(f => f.Id.HasValue)
                .ToDictionary(f => f.Id!.Value, f => f.Nome ?? f.Fantasia ?? $"Fornecedor {f.Id}");

            var colaboradores = _context.Colaboradores.AsNoTracking()
                .Where(c => c.Id.HasValue)
                .ToDictionary(c => c.Id!.Value, c => c.Nome ?? $"Colaborador {c.Id}");

            return _context.CompraPedidosCabecalho.AsNoTracking()
                .OrderByDescending(c => c.DataPedido)
                .Take(500)
                .AsEnumerable()
                .Select(c => new CompraPedidoDto(
                    c.Id,
                    c.IdColaborador.HasValue && colaboradores.TryGetValue(c.IdColaborador.Value, out var colaborador) ? colaborador : string.Empty,
                    c.IdFornecedor.HasValue && fornecedores.TryGetValue(c.IdFornecedor.Value, out var fornecedor) ? fornecedor : "Fornecedor nao informado",
                    c.DataPedido,
                    c.DataPrevisaoEntrega,
                    c.DataPrevisaoPagamento,
                    c.LocalEntrega ?? string.Empty,
                    c.LocalCobranca ?? string.Empty,
                    c.Contato ?? string.Empty))
                .ToList();
        }

        public CompraPedidoDto SalvarPedidoCompra(CompraPedidoInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Fornecedor))
                throw new InvalidOperationException("Fornecedor e obrigatorio.");

            using var transaction = _context.Database.BeginTransaction();

            var fornecedorId = ResolverFornecedorId(input.Fornecedor);
            var colaboradorId = ResolverColaboradorId(input.Colaborador);
            var total = input.ValorTotal > 0 ? input.ValorTotal : input.ValorSubtotal - input.ValorDesconto;

            var pedido = new CompraPedidoCabecalho
            {
                Id = null,
                IdColaborador = colaboradorId,
                IdFornecedor = fornecedorId,
                DataPedido = input.DataPedido ?? DateTime.Today,
                DataPrevisaoEntrega = input.DataPrevistaEntrega,
                DataPrevisaoPagamento = input.DataPrevisaoPagamento,
                LocalEntrega = input.LocalEntrega,
                LocalCobranca = input.LocalCobranca,
                Contato = input.NomeContato,
                ValorSubtotal = ToDouble(input.ValorSubtotal),
                TaxaDesconto = ToDouble(input.TaxaDesconto),
                ValorDesconto = ToDouble(input.ValorDesconto),
                ValorTotal = ToDouble(total),
                FormaPagamento = input.CondicaoPagamento,
                GeraFinanceiro = "S",
                QuantidadeParcelas = input.QuantidadeParcelas <= 0 ? 1 : input.QuantidadeParcelas,
                DiaPrimeiroVencimento = input.DiaPrimeiroVencimento ?? input.DataPrevisaoPagamento ?? DateTime.Today,
                IntervaloEntreParcelas = input.IntervaloParcelas <= 0 ? 30 : input.IntervaloParcelas,
                DiaFixoParcela = input.DiaFixoParcela,
                DataRecebimentoItens = input.DataRecebimentoItens,
                HoraRecebimentoItens = input.HoraRecebimentoItens,
                AtualizouEstoque = "N",
                NumeroDocumentoEntrada = input.NumeroDocumentoEntrada
            };

            _context.CompraPedidosCabecalho.Add(pedido);
            _context.SaveChanges();

            if (total > 0)
            {
                _context.ContasPagar.Add(new ContasPagar
                {
                    Id = null,
                    IdFornecedor = fornecedorId,
                    IdCompraPedidoCabecalho = pedido.Id,
                    DataLancamento = pedido.DataPedido,
                    DataVencimento = pedido.DiaPrimeiroVencimento,
                    ValorAPagar = ToDouble(total),
                    ValorPago = 0,
                    TaxaJuro = 0,
                    ValorJuro = 0,
                    NumeroDocumento = pedido.NumeroDocumentoEntrada ?? pedido.Id?.ToString(),
                    Historico = $"Pedido de compra #{pedido.Id}",
                    StatusPagamento = "A Pagar"
                });
            }

            _context.SaveChanges();
            transaction.Commit();
            SincronizarRetaguardaSeAutenticado();

            return new CompraPedidoDto(
                pedido.Id,
                input.Colaborador ?? string.Empty,
                input.Fornecedor,
                pedido.DataPedido,
                pedido.DataPrevisaoEntrega,
                pedido.DataPrevisaoPagamento,
                pedido.LocalEntrega ?? string.Empty,
                pedido.LocalCobranca ?? string.Empty,
                pedido.Contato ?? string.Empty);
        }

        private int? ResolverFornecedorId(string fornecedor)
        {
            var texto = fornecedor.Trim();
            return _context.Fornecedores.AsNoTracking()
                .Where(f => f.Nome == texto || f.Fantasia == texto || f.CpfCnpj == texto)
                .Select(f => f.Id)
                .FirstOrDefault();
        }

        private int? ResolverColaboradorId(string? colaborador)
        {
            if (string.IsNullOrWhiteSpace(colaborador))
                return null;

            var texto = colaborador.Trim();
            return _context.Colaboradores.AsNoTracking()
                .Where(c => c.Nome == texto)
                .Select(c => c.Id)
                .FirstOrDefault();
        }

        private int? ObterOuCriarTipoPagamentoId(string tipoPagamento)
        {
            var texto = string.IsNullOrWhiteSpace(tipoPagamento) ? "Dinheiro" : tipoPagamento.Trim();
            var existente = ResolverTipoPagamentoId(texto);
            if (existente.HasValue)
                return existente;

            var tipo = new PdvTipoPagamento
            {
                Id = null,
                Codigo = GerarCodigoTipoPagamento(texto),
                Descricao = texto,
                PermiteTroco = texto.Contains("Dinheiro", StringComparison.OrdinalIgnoreCase) ? "S" : "N",
                GeraParcelas = EhPagamentoPendente(texto) ? "S" : "N",
                Tef = texto.Contains("Cartao", StringComparison.OrdinalIgnoreCase)
                    || texto.Contains("Cartão", StringComparison.OrdinalIgnoreCase)
                    ? "S"
                    : "N",
                CodigoPagamentoNfce = ResolverCodigoPagamentoNfce(texto)
            };

            _context.PdvTiposPagamento.Add(tipo);
            _context.SaveChanges();
            return tipo.Id;
        }

        private int? ResolverTipoPagamentoId(string? tipoPagamento)
        {
            if (string.IsNullOrWhiteSpace(tipoPagamento))
                return null;

            var texto = tipoPagamento.Trim();
            return _context.PdvTiposPagamento.AsNoTracking()
                .Where(t => t.Descricao == texto || t.Codigo == texto)
                .Select(t => t.Id)
                .FirstOrDefault();
        }

        private static string GerarCodigoTipoPagamento(string tipoPagamento)
        {
            var codigo = new string((tipoPagamento ?? string.Empty)
                .Where(char.IsLetterOrDigit)
                .Take(12)
                .ToArray())
                .ToUpperInvariant();

            return string.IsNullOrWhiteSpace(codigo) ? "DINHEIRO" : codigo;
        }

        private static string ResolverCodigoPagamentoNfce(string tipoPagamento)
        {
            if (tipoPagamento.Contains("Dinheiro", StringComparison.OrdinalIgnoreCase))
                return "01";
            if (tipoPagamento.Contains("Credito", StringComparison.OrdinalIgnoreCase) || tipoPagamento.Contains("Crédito", StringComparison.OrdinalIgnoreCase))
                return "03";
            if (tipoPagamento.Contains("Debito", StringComparison.OrdinalIgnoreCase) || tipoPagamento.Contains("Débito", StringComparison.OrdinalIgnoreCase))
                return "04";
            if (tipoPagamento.Contains("Pix", StringComparison.OrdinalIgnoreCase))
                return "17";
            if (tipoPagamento.Contains("Vale", StringComparison.OrdinalIgnoreCase))
                return "10";

            return "99";
        }

        private static string? ResolverCartaoDc(string tipoPagamento)
        {
            if (tipoPagamento.Contains("Credito", StringComparison.OrdinalIgnoreCase) || tipoPagamento.Contains("Crédito", StringComparison.OrdinalIgnoreCase))
                return "C";
            if (tipoPagamento.Contains("Debito", StringComparison.OrdinalIgnoreCase) || tipoPagamento.Contains("Débito", StringComparison.OrdinalIgnoreCase))
                return "D";

            return null;
        }

        private static bool EhPagamentoPendente(string tipoPagamento)
        {
            return tipoPagamento.Contains("Fiado", StringComparison.OrdinalIgnoreCase)
                || tipoPagamento.Contains("Prazo", StringComparison.OrdinalIgnoreCase)
                || tipoPagamento.Contains("A prazo", StringComparison.OrdinalIgnoreCase);
        }

        private static VendaResumoDto MapVenda(PdvVendaCabecalho venda)
        {
            TimeSpan? hora = null;
            if (TimeSpan.TryParse(venda.HoraVenda, out var parsed))
                hora = parsed;

            return new VendaResumoDto(
                venda.Id,
                venda.IdPdvMovimento,
                venda.DataVenda,
                hora,
                ToDecimal(venda.ValorVenda),
                ToDecimal(venda.TaxaDesconto),
                ToDecimal(venda.ValorDesconto),
                ToDecimal(venda.ValorFinal),
                ToDecimal(venda.ValorRecebido),
                ToDecimal(venda.ValorTroco),
                MapStatusVenda(venda.StatusVenda));
        }

        private static string MapStatusVenda(string? status)
        {
            return status switch
            {
                "F" => "Fechada",
                "A" => "Aberta",
                "C" => "Cancelada",
                _ => status ?? "Nao informado"
            };
        }

        private static decimal ToDecimal(double? value) => Convert.ToDecimal(value ?? 0);

        private static double ToDouble(decimal value) => Convert.ToDouble(value);

        private void SincronizarRetaguardaSeAutenticado()
        {
            var resultado = _syncService.SincronizarTudoAsync().GetAwaiter().GetResult();
            if (!resultado.Sincronizado && string.Equals(Environment.GetEnvironmentVariable("PDV_SYNC_STRICT"), "true", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(resultado.Mensagem);
        }

        private void EmitirNfceSeConfigurado(int? vendaId)
        {
            if (!vendaId.HasValue || _fiscalNfceService == null)
                return;

            var habilitado = Environment.GetEnvironmentVariable("PDV_FISCAL_ENABLED");
            if (!string.Equals(habilitado, "true", StringComparison.OrdinalIgnoreCase))
                return;

            var resultado = _fiscalNfceService.EmitirNfceVendaAsync(vendaId.Value).GetAwaiter().GetResult();
            if (!resultado.Sucesso && string.Equals(Environment.GetEnvironmentVariable("PDV_FISCAL_STRICT"), "true", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Falha na emissao NFC-e da venda {vendaId}: {resultado.Mensagem}");
        }
    }
}

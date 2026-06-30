using Microsoft.Extensions.DependencyInjection;
using PDV.Models.Pdv;
using PDV.Models.Pdv.Cadastros;
using PDV.Services.Interfaces;
using PDV.ViewModels;
using System.Globalization;
using System.IO;

namespace PDV.Services.Diagnostics
{
    public static class CaixaOperationalSmokeRunner
    {
        private static readonly string[] Modalidades =
        {
            "Dinheiro",
            "Cartao Credito",
            "Cartao Debito",
            "Pix",
            "Vale Alimentacao"
        };

        public static int Run(ServiceProvider serviceProvider, string runRoot)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            Directory.CreateDirectory(runRoot);
            var reportPath = Path.Combine(runRoot, "caixa-operacional-smoke.txt");
            var report = new List<string>
            {
                $"Inicio: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                "Objetivo: abertura, vendas, pagamentos, fiado, cardapio, 180 vendas, fechamento e relatorios."
            };

            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
                var pdv = scope.ServiceProvider.GetRequiredService<IPdvOperationService>();

                var cliente = CriarCliente(context);
                var produtos = CriarProdutos(context, 12);
                var cardapioProdutos = CriarProdutosCardapio(context, 6);
                CriarTiposPagamento(context);
                report.Add($"Preparacao OK: cliente #{cliente.Id}, {produtos.Count} produtos, {cardapioProdutos.Count} itens de cardapio e {Modalidades.Length + 2} tipos de pagamento.");

                ValidarAberturaCaixa(pdv, context, report);
                ValidarViewModelsOperacionais(scope.ServiceProvider, context, produtos, report);
                ValidarVendasDiretasPorModalidade(pdv, context, produtos, report);
                ValidarVendaSemCliente(pdv, context, produtos, report);
                ValidarVendaComCliente(pdv, context, cliente, produtos, report);
                ValidarVendaPrazoClienteFiado(pdv, context, cliente, produtos, report);
                ValidarVendaNormalClienteFiado(pdv, context, cliente, produtos, report);
                ValidarVendaVariosProdutos(pdv, context, cliente, produtos, report);
                ValidarVendaVariosItensCardapio(pdv, context, cliente, cardapioProdutos, report);
                ValidarCentoEOitentaVendas(pdv, context, cliente, produtos, cardapioProdutos, report);
                ValidarFechamentoCaixa(scope.ServiceProvider, pdv, context, report);
                ValidarRelatorios(pdv, context, report);

                report.Add($"Fim: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                report.Add("RESULTADO: OK");
                File.WriteAllLines(reportPath, report);
                return 0;
            }
            catch (Exception ex)
            {
                report.Add($"ERRO: {ex}");
                report.Add("RESULTADO: FALHOU");
                File.WriteAllLines(reportPath, report);
                return 1;
            }
        }

        private static void ValidarAberturaCaixa(IPdvOperationService pdv, PdvContext context, List<string> report)
        {
            Require(pdv.ObterMovimentoAberto() == null, "Consulta de movimento criou um caixa indevidamente.");
            Require(!pdv.ExisteMovimentoAberto(), "Verificacao de estado criou um caixa indevidamente.");
            Require(context.PdvMovimentos.Count() == 0, "Existe movimento antes da abertura explicita.");

            EsperarErro(() => pdv.AbrirMovimento(-1), "Abertura rejeita fundo negativo.");
            EsperarErro(() => pdv.AbrirMovimento(100001), "Abertura rejeita fundo irreal.");

            var movimento = pdv.AbrirMovimento(250m);
            Require(movimento.Id.HasValue, "Abertura nao gerou movimento.");
            Require(movimento.StatusMovimento == "A", "Movimento nao abriu com status A.");
            Require(context.PdvSuprimentos.Any(s => s.IdPdvMovimento == movimento.Id && Math.Abs((s.Valor ?? 0) - 250d) < 0.001), "Fundo de troco nao foi registrado como suprimento.");

            var segundaAbertura = pdv.AbrirMovimento(999m);
            Require(segundaAbertura.Id == movimento.Id, "Abertura duplicada criou outro movimento.");
            Require(context.PdvMovimentos.Count(m => m.DataFechamento == null) == 1, "Existe mais de um movimento aberto.");

            pdv.RegistrarSuprimento(40m, "Teste suprimento caixa");
            pdv.RegistrarSangria(25m, "Teste sangria caixa");
            Require(context.PdvSuprimentos.Count(s => s.IdPdvMovimento == movimento.Id) >= 2, "Suprimento operacional nao foi gravado.");
            Require(context.PdvSangrias.Any(s => s.IdPdvMovimento == movimento.Id), "Sangria operacional nao foi gravada.");

            report.Add($"Abertura OK: movimento #{movimento.Id}, fundo, bloqueios, suprimento e sangria validados.");
        }

        private static void ValidarViewModelsOperacionais(IServiceProvider provider, PdvContext context, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var movimento = context.PdvMovimentos.OrderByDescending(m => m.Id).First();
            var pdv = provider.GetRequiredService<IPdvOperationService>();
            var navigation = provider.GetRequiredService<IViewModelNavigationService>();

            pdv.FecharMovimento(250m, "Dinheiro");

            var abertura = provider.GetRequiredService<AberturaCaixaViewModel>();
            abertura.FundoTrocoTexto = string.Empty;
            abertura.ConfirmarCommand.Execute(null);
            Require(context.PdvMovimentos.Count(m => m.DataFechamento == null) == 0, "Abertura VM aceitou campo vazio e alterou o caixa.");

            abertura.FundoTrocoTexto = "abc";
            abertura.ConfirmarCommand.Execute(null);
            Require(context.PdvMovimentos.Count(m => m.DataFechamento == null) == 0, "Abertura VM aceitou letras e alterou o caixa.");

            abertura = provider.GetRequiredService<AberturaCaixaViewModel>();
            abertura.FundoTrocoTexto = "50,00";
            abertura.ConfirmarCommand.Execute(null);
            movimento = context.PdvMovimentos.OrderByDescending(m => m.Id).First();
            Require(context.PdvMovimentos.Count(m => m.DataFechamento == null) == 1, "Abertura VM nao deixou exatamente um caixa aberto.");
            Require(navigation.CurrentViewModel is HomeViewModel, "Abertura VM nao navegou para Caixa/Venda apos abrir caixa.");
            var fechamentoPreview = provider.GetRequiredService<AberturaCaixaViewModel>();
            Require(fechamentoPreview.IsCaixaAberto, "Fechamento VM nao reconheceu caixa aberto.");
            Require(fechamentoPreview.TotaisPagamento.Any(l => l.FormaPagamento == "Dinheiro em caixa"), "Fechamento VM nao exibiu total de dinheiro em caixa.");

            var suprimento = provider.GetRequiredService<SuprimentoViewModel>();
            suprimento.ValorTexto = "15,50";
            suprimento.Observacao = "VM suprimento";
            suprimento.ConfirmarCommand.Execute(null);
            Require(context.PdvSuprimentos.Any(s => s.IdPdvMovimento == movimento.Id && s.Observacao == "VM suprimento" && Math.Abs((s.Valor ?? 0) - 15.5d) < 0.001), "Suprimento VM nao gravou pelo ConfirmarCommand.");

            var sangria = provider.GetRequiredService<SangriaViewModel>();
            sangria.ValorTexto = "7,25";
            sangria.Observacao = "VM sangria";
            sangria.ConfirmarCommand.Execute(null);
            Require(context.PdvSangrias.Any(s => s.IdPdvMovimento == movimento.Id && s.Observacao == "VM sangria" && Math.Abs((s.Valor ?? 0) - 7.25d) < 0.001), "Sangria VM nao gravou pelo ConfirmarCommand.");

            var vendas = provider.GetRequiredService<VendasViewModel>();
            vendas.TermoProduto = "Produto Caixa";
            vendas.BuscarProdutoCommand.Execute(null);
            Require(vendas.ProdutosEncontrados.Count > 0, "Vendas VM nao encontrou produtos cadastrados.");
            vendas.ProdutoSelecionado = vendas.ProdutosEncontrados.First(p => p.Id == produtos[0].Id);
            vendas.QuantidadeTexto = "2";
            vendas.AdicionarItemCommand.Execute(null);
            Require(vendas.ItensVenda.Count == 1 && vendas.TotalVenda > 0, "Vendas VM nao adicionou item pelo botao/comando.");
            vendas.TipoPagamentoSelecionado = "Pix";
            vendas.ValorRecebidoTexto = vendas.TotalVenda.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));
            var vendasAntes = context.PdvVendasCabecalho.Count();
            vendas.ConcluirVendaCommand.Execute(null);
            Require(vendas.MostrarPagamento, "Vendas VM nao abriu a etapa de pagamento ao concluir venda.");
            vendas.TipoPagamentoSelecionado = "Fiado";
            Require(!vendas.MostrarVencimentoFiadoPrazo, "Vendas VM exibiu vencimento fiado/prazo sem cliente cadastrado.");
            vendas.ConfirmarPagamentoCommand.Execute(null);
            Require(context.PdvVendasCabecalho.Count() == vendasAntes, "Vendas VM gravou fiado sem cliente cadastrado.");
            vendas.ClienteIdTexto = "1";
            Require(vendas.MostrarVencimentoFiadoPrazo, "Vendas VM nao exibiu vencimento fiado/prazo com cliente cadastrado.");
            vendas.TipoPagamentoSelecionado = "Pix";
            Require(context.PdvVendasCabecalho.Count() == vendasAntes, "Venda foi gravada antes de confirmar pagamento.");
            vendas.ConfirmarPagamentoCommand.Execute(null);
            Require(context.PdvVendasCabecalho.Count() == vendasAntes + 1, "Vendas VM nao gravou venda ao confirmar pagamento.");
            Require(vendas.Vendas.Any(v => v.Id.HasValue), "Venda confirmada nao apareceu na relacao de vendas.");

            var home = provider.GetRequiredService<HomeViewModel>();
            home.TermoBusca = "Produto Caixa";
            home.PesquisarCommand.Execute(null);
            Require(home.ProdutosEncontrados.Count > 0, "Home/Caixa VM nao encontrou produtos cadastrados.");
            home.ProdutoSelecionado = home.ProdutosEncontrados.First(p => p.ProdutoId == produtos[1].Id);
            home.AdicionarProdutoCommand.Execute(null);
            Require(home.Itens.Count == 1 && home.TotalVenda > 0, "Home/Caixa VM nao adicionou item na venda.");
            home.TipoPagamentoSelecionado = "Dinheiro";
            home.ValorRecebidoTexto = (home.TotalVenda + 10m).ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));
            var vendasAntesHome = context.PdvVendasCabecalho.Count();
            home.ConcluirVendaCommand.Execute(null);
            Require(home.MostrarPagamento, "Home/Caixa VM nao abriu a etapa de pagamento ao concluir venda.");
            home.TipoPagamentoSelecionado = "Fiado";
            Require(!home.MostrarVencimentoFiadoPrazo, "Home/Caixa VM exibiu vencimento fiado/prazo sem cliente cadastrado.");
            home.ConfirmarPagamentoCommand.Execute(null);
            Require(context.PdvVendasCabecalho.Count() == vendasAntesHome, "Home/Caixa VM gravou fiado sem cliente cadastrado.");
            home.TipoPagamentoSelecionado = "Dinheiro";
            Require(context.PdvVendasCabecalho.Count() == vendasAntesHome, "Home/Caixa gravou venda antes de confirmar pagamento.");
            home.ConfirmarPagamentoCommand.Execute(null);
            Require(context.PdvVendasCabecalho.Count() == vendasAntesHome + 1, "Home/Caixa VM nao gravou venda ao confirmar pagamento.");
            Require(context.PdvVendasCabecalho.OrderByDescending(v => v.Id).First().ValorTroco > 0, "Home/Caixa VM nao calculou/persistiu troco em dinheiro.");
            Require(pdv.ListarVendas(DateTime.Today, "Fechadas").Any(v => v.Id.HasValue), "Venda do Home/Caixa nao apareceu na relacao de vendas.");

            var movimentoVm = provider.GetRequiredService<MovimentoViewModel>();
            Require(movimentoVm.Linhas.Any(l => l.Tipo == "Venda"), "Movimento VM nao exibiu vendas do caixa.");
            Require(movimentoVm.Linhas.Any(l => l.Tipo == "Suprimento"), "Movimento VM nao exibiu suprimentos do caixa.");
            Require(movimentoVm.Linhas.Any(l => l.Tipo == "Sangria"), "Movimento VM nao exibiu sangrias do caixa.");
            Require(movimentoVm.StatusMovimento == "Aberto", "Movimento VM nao identificou o caixa aberto.");

            report.Add("ViewModels OK: abertura navega para Caixa/Venda; vencimento fiado/prazo exige cliente cadastrado; sangria, suprimento, Vendas, Home/Caixa e extrato de Movimento validados.");
        }

        private static void ValidarVendasDiretasPorModalidade(IPdvOperationService pdv, PdvContext context, IReadOnlyList<Produto> produtos, List<string> report)
        {
            foreach (var modalidade in Modalidades)
            {
                var produto = produtos[Array.IndexOf(Modalidades, modalidade) % produtos.Count];
                var total = Valor(produto);
                var venda = pdv.FinalizarVenda(new VendaInput(
                    new[] { Item(produto, 1) },
                    new[] { new VendaPagamentoDto(modalidade, total, Nsu: $"NSU-{modalidade}") }));

                RequirePagamento(context, venda.Id, modalidade, total);
                RequireRecebido(context, venda.Id);
            }

            report.Add($"Venda direta OK: modalidades testadas ({string.Join(", ", Modalidades)}).");
        }

        private static void ValidarVendaSemCliente(IPdvOperationService pdv, PdvContext context, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var produto = produtos[0];
            var venda = pdv.FinalizarVenda(new VendaInput(
                new[] { Item(produto, 2) },
                new[] { new VendaPagamentoDto("Dinheiro", Valor(produto) * 2) }));

            var cabecalho = context.PdvVendasCabecalho.First(v => v.Id == venda.Id);
            Require(cabecalho.IdCliente == null, "Venda sem cliente gravou cliente.");
            RequireRecebido(context, venda.Id);
            report.Add($"Venda sem cliente OK: venda #{venda.Id}.");
        }

        private static void ValidarVendaComCliente(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var produto = produtos[1];
            var venda = pdv.FinalizarVenda(new VendaInput(
                new[] { Item(produto, 1) },
                new[] { new VendaPagamentoDto("Pix", Valor(produto)) },
                cliente.Id));

            var cabecalho = context.PdvVendasCabecalho.First(v => v.Id == venda.Id);
            Require(cabecalho.IdCliente == cliente.Id, "Venda para cliente nao vinculou cliente.");
            RequireRecebido(context, venda.Id);
            report.Add($"Venda para cliente OK: venda #{venda.Id}, cliente #{cliente.Id}.");
        }

        private static void ValidarVendaPrazoClienteFiado(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var produto = produtos[2];
            var total = Valor(produto);
            var venda = pdv.FinalizarVenda(new VendaInput(
                new[] { Item(produto, 1) },
                new[] { new VendaPagamentoDto("A Prazo", total, DateTime.Today.AddDays(20)) },
                cliente.Id,
                ClienteFiado: true));

            RequireFiado(context, venda.Id, cliente.Id, total);
            report.Add($"Venda a prazo para cliente fiado OK: venda #{venda.Id}.");
        }

        private static void ValidarVendaNormalClienteFiado(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var produto = produtos[3];
            var total = Valor(produto);
            var venda = pdv.FinalizarVenda(new VendaInput(
                new[] { Item(produto, 1) },
                new[] { new VendaPagamentoDto("Fiado", total, DateTime.Today.AddDays(30)) },
                cliente.Id,
                ClienteFiado: true));

            RequireFiado(context, venda.Id, cliente.Id, total);
            report.Add($"Venda normal para cliente fiado OK: venda #{venda.Id}.");
        }

        private static void ValidarVendaVariosProdutos(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtos, List<string> report)
        {
            var itens = produtos.Take(5).Select(p => Item(p, 1)).ToList();
            var total = itens.Sum(i => i.Quantidade * i.ValorUnitario);
            var venda = pdv.FinalizarVenda(new VendaInput(
                itens,
                new[]
                {
                    new VendaPagamentoDto("Dinheiro", total / 2),
                    new VendaPagamentoDto("Cartao Debito", total / 2, Nsu: "NSU-MULTI")
                },
                cliente.Id));

            Require(context.PdvVendasDetalhe.Count(d => d.IdPdvVendaCabecalho == venda.Id) == 5, "Venda com varios produtos nao gravou todos os itens.");
            Require(context.PdvTotaisTipoPagamento.Count(p => p.IdPdvVendaCabecalho == venda.Id) == 2, "Venda mista nao gravou dois pagamentos.");
            report.Add($"Venda varios produtos OK: venda #{venda.Id}, 5 itens e 2 pagamentos.");
        }

        private static void ValidarVendaVariosItensCardapio(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtosCardapio, List<string> report)
        {
            var itens = produtosCardapio.Take(4).Select(p => Item(p, 1)).ToList();
            var total = itens.Sum(i => i.Quantidade * i.ValorUnitario);
            var venda = pdv.FinalizarVenda(new VendaInput(
                itens,
                new[] { new VendaPagamentoDto("Pix", total) },
                cliente.Id));

            var idsProdutos = itens.Select(i => i.ProdutoId).ToHashSet();
            Require(context.Cardapios.Count(c => c.IdProduto.HasValue && idsProdutos.Contains(c.IdProduto)) == 4, "Itens de cardapio nao estavam vinculados ao cadastro CARDAPIO.");
            Require(context.PdvVendasDetalhe.Count(d => d.IdPdvVendaCabecalho == venda.Id) == 4, "Venda de cardapio nao gravou todos os itens.");
            report.Add($"Venda cardapio OK: venda #{venda.Id}, 4 itens de cardapio.");
        }

        private static void ValidarCentoEOitentaVendas(IPdvOperationService pdv, PdvContext context, Cliente cliente, IReadOnlyList<Produto> produtos, IReadOnlyList<Produto> cardapioProdutos, List<string> report)
        {
            var inicio = context.PdvVendasCabecalho.Count();
            var todosProdutos = produtos.Concat(cardapioProdutos).ToList();

            for (var i = 1; i <= 180; i++)
            {
                var produto = todosProdutos[i % todosProdutos.Count];
                var quantidade = (i % 3) + 1;
                var total = Valor(produto) * quantidade;
                var modalidade = Modalidades[i % Modalidades.Length];
                var comCliente = i % 2 == 0;
                var fiado = i % 30 == 0;
                var tipo = fiado ? "Fiado" : modalidade;

                var venda = pdv.FinalizarVenda(new VendaInput(
                    new[] { Item(produto, quantidade) },
                    new[] { new VendaPagamentoDto(tipo, total, fiado ? DateTime.Today.AddDays(30) : null, Nsu: $"NSU-180-{i:000}") },
                    comCliente || fiado ? cliente.Id : null,
                    ClienteFiado: fiado));

                Require(venda.Id.HasValue, $"Venda em massa {i} nao retornou Id.");
            }

            var criadas = context.PdvVendasCabecalho.Count() - inicio;
            Require(criadas == 180, $"Esperava 180 vendas em massa; gerou {criadas}.");
            Require(context.PdvVendasDetalhe.Count() >= 180, "Vendas em massa nao geraram detalhes suficientes.");
            report.Add("180 vendas OK: volume, pagamentos rotativos, clientes alternados e fiado a cada 30 vendas.");
        }

        private static void ValidarFechamentoCaixa(IServiceProvider provider, IPdvOperationService pdv, PdvContext context, List<string> report)
        {
            var movimento = pdv.ObterMovimentoAberto()
                ?? throw new InvalidOperationException("Movimento sumiu antes do fechamento.");

            var vendasMovimento = context.PdvVendasCabecalho.Where(v => v.IdPdvMovimento == movimento.Id).ToList();
            var totalFinal = vendasMovimento.Sum(v => Convert.ToDecimal(v.ValorFinal ?? 0));
            var totalSuprimento = Convert.ToDecimal(movimento.TotalSuprimento ?? 0);
            var totalSangria = Convert.ToDecimal(movimento.TotalSangria ?? 0);
            var valorInformado = totalFinal + totalSuprimento - totalSangria;

            var fechamentoVm = provider.GetRequiredService<AberturaCaixaViewModel>();
            Require(fechamentoVm.IsCaixaAberto, "Tela de fechamento nao abriu em modo caixa aberto.");
            Require(fechamentoVm.TotalCaixaTexto.Contains(valorInformado.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"))), "Tela de fechamento nao exibiu o total do caixa esperado.");
            Require(fechamentoVm.TotaisPagamento.Any(t => t.FormaPagamento == "Dinheiro em caixa"), "Tela de fechamento nao exibiu dinheiro.");
            Require(fechamentoVm.TotaisPagamento.Any(t => t.FormaPagamento == "Debito"), "Tela de fechamento nao exibiu debito.");
            Require(fechamentoVm.TotaisPagamento.Any(t => t.FormaPagamento == "Credito"), "Tela de fechamento nao exibiu credito.");
            Require(fechamentoVm.TotaisPagamento.Any(t => t.FormaPagamento == "Pix"), "Tela de fechamento nao exibiu pix.");

            fechamentoVm.ConfirmarCommand.Execute(null);
            context.ChangeTracker.Clear();
            var fechado = context.PdvMovimentos.OrderByDescending(m => m.Id).First();
            Require(fechado.StatusMovimento == "F", "Fechamento nao mudou status para F.");
            Require(fechado.DataFechamento.HasValue, "Fechamento nao preencheu data.");
            Require(context.PdvFechamentos.Any(f => f.IdPdvMovimento == fechado.Id), "Fechamento nao gravou PDV_FECHAMENTO.");
            Require(pdv.ObterMovimentoAberto() == null, "Ainda existe movimento aberto apos fechamento.");

            report.Add($"Fechamento OK: movimento #{fechado.Id}, {vendasMovimento.Count} vendas, valor informado {valorInformado:N2}.");
        }

        private static void ValidarRelatorios(IPdvOperationService pdv, PdvContext context, List<string> report)
        {
            var vendas = pdv.ListarVendas(DateTime.Today, "Fechadas");
            var estoque = pdv.ListarEstoque();
            var contasReceber = pdv.ListarContasReceber();
            var totalPagamentos = context.PdvTotaisTipoPagamento.Count();
            var totalFiados = context.ClientesFiados.Count();

            Require(vendas.Count >= 180, "Relatorio de vendas fechadas nao retornou volume esperado.");
            Require(estoque.Count > 0, "Relatorio/listagem de estoque vazio.");
            Require(contasReceber.Count > 0, "Relatorio/listagem de contas a receber vazio.");
            Require(totalPagamentos >= 180, "Relatorio interno de pagamentos nao tem registros suficientes.");
            Require(totalFiados >= 2, "Relatorio interno de fiado nao tem registros esperados.");

            report.Add($"Relatorios OK: vendas={vendas.Count}, estoque={estoque.Count}, contasReceber={contasReceber.Count}, pagamentos={totalPagamentos}, fiados={totalFiados}.");
        }

        private static Cliente CriarCliente(PdvContext context)
        {
            var cliente = new Cliente
            {
                Id = null,
                Nome = "Cliente Caixa Smoke",
                Fantasia = "Cliente Caixa",
                Email = "cliente.caixa@local.com",
                CpfCnpj = "12345678910",
                TipoPessoa = "F",
                DataCadastro = DateTime.Today,
                FiadoValorTeto = 100000
            };

            context.Clientes.Add(cliente);
            context.SaveChanges();
            return cliente;
        }

        private static IReadOnlyList<Produto> CriarProdutos(PdvContext context, int quantidade)
        {
            var produtos = new List<Produto>();
            for (var i = 1; i <= quantidade; i++)
            {
                var produto = new Produto
                {
                    Id = null,
                    Gtin = $"789000000{i:0000}",
                    CodigoInterno = $"CX-{i:000}",
                    Nome = $"Produto Caixa {i:000}",
                    Descricao = $"Produto Caixa {i:000}",
                    DescricaoPdv = $"Produto Caixa {i:000}",
                    ValorCompra = 3 + i,
                    ValorVenda = 7.5 + i,
                    QuantidadeEstoque = 10000,
                    EstoqueMinimo = 1,
                    EstoqueMaximo = 20000,
                    Situacao = "Ativo"
                };
                context.Produtos.Add(produto);
                produtos.Add(produto);
            }

            context.SaveChanges();
            return produtos;
        }

        private static IReadOnlyList<Produto> CriarProdutosCardapio(PdvContext context, int quantidade)
        {
            var produtos = CriarProdutos(context, quantidade)
                .Select((p, i) =>
                {
                    p.CodigoInterno = $"MENU-{i + 1:000}";
                    p.Nome = $"Cardapio Item {i + 1:000}";
                    p.DescricaoPdv = $"Cardapio Item {i + 1:000}";
                    return p;
                })
                .ToList();

            foreach (var produto in produtos)
            {
                context.Cardapios.Add(new Cardapio
                {
                    Id = null,
                    IdProduto = produto.Id,
                    ModoPreparo = $"Preparo {produto.Nome}",
                    Ingredientes = "Ingrediente A, Ingrediente B",
                    InfoAlergico = "Sem alerta"
                });
            }

            context.SaveChanges();
            return produtos;
        }

        private static void CriarTiposPagamento(PdvContext context)
        {
            foreach (var modalidade in Modalidades.Concat(new[] { "A Prazo", "Fiado" }))
            {
                if (context.PdvTiposPagamento.Any(p => p.Descricao == modalidade))
                    continue;

                context.PdvTiposPagamento.Add(new PdvTipoPagamento
                {
                    Id = null,
                    Codigo = new string(modalidade.Where(char.IsLetterOrDigit).Take(12).ToArray()).ToUpperInvariant(),
                    Descricao = modalidade,
                    PermiteTroco = modalidade == "Dinheiro" ? "S" : "N",
                    GeraParcelas = modalidade.Contains("Prazo") || modalidade.Contains("Fiado") ? "S" : "N",
                    Tef = modalidade.Contains("Cartao") ? "S" : "N"
                });
            }

            context.SaveChanges();
        }

        private static VendaItemDto Item(Produto produto, decimal quantidade)
        {
            return new VendaItemDto(
                produto.Id,
                produto.Gtin ?? string.Empty,
                produto.DescricaoPdv ?? produto.Nome ?? "Produto",
                quantidade,
                Valor(produto),
                0);
        }

        private static decimal Valor(Produto produto)
        {
            return Convert.ToDecimal(produto.ValorVenda ?? 1);
        }

        private static void RequirePagamento(PdvContext context, int? vendaId, string modalidade, decimal valor)
        {
            var tipoId = context.PdvTiposPagamento.Where(t => t.Descricao == modalidade).Select(t => t.Id).FirstOrDefault();
            Require(tipoId.HasValue, $"Tipo de pagamento {modalidade} nao existe.");
            Require(context.PdvTotaisTipoPagamento.Any(p =>
                p.IdPdvVendaCabecalho == vendaId &&
                p.IdPdvTipoPagamento == tipoId &&
                Math.Abs((p.Valor ?? 0) - Convert.ToDouble(valor)) < 0.01), $"Pagamento {modalidade} nao foi gravado para venda {vendaId}.");
        }

        private static void RequireRecebido(PdvContext context, int? vendaId)
        {
            Require(context.ContasReceber.Any(c => c.IdPdvVendaCabecalho == vendaId && c.StatusRecebimento == "Recebido" && (c.ValorRecebido ?? 0) > 0), $"Conta recebida nao foi gravada para venda {vendaId}.");
        }

        private static void RequireFiado(PdvContext context, int? vendaId, int? clienteId, decimal valor)
        {
            Require(context.ContasReceber.Any(c => c.IdPdvVendaCabecalho == vendaId && c.IdCliente == clienteId && c.StatusRecebimento == "A Receber" && (c.ValorRecebido ?? 0) == 0), $"Conta a receber pendente nao foi gravada para venda fiado {vendaId}.");
            Require(context.ClientesFiados.Any(f => f.IdPdvVendaCabecalho == vendaId && f.IdCliente == clienteId && Math.Abs((f.ValorPendente ?? 0) - Convert.ToDouble(valor)) < 0.01), $"CLIENTE_FIADO nao foi gravado para venda {vendaId}.");
        }

        private static void EsperarErro(Action action, string descricao)
        {
            try
            {
                action();
            }
            catch
            {
                return;
            }

            throw new InvalidOperationException($"{descricao} Nao gerou erro.");
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}

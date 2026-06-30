using Microsoft.Extensions.DependencyInjection;
using PDV.Models.Pdv.Cadastros;
using PDV.Services.Interfaces;
using PDV.ViewModels;
using PDV.Views;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CadClienteListView = PDV.Views.Cadastros.ClienteListView;
using CadColaboradorListView = PDV.Views.Cadastros.ColaboradorListView;
using CadEmpresaListView = PDV.Views.Cadastros.EmpresaListView;
using CadFormaPagamentoListView = PDV.Views.Cadastros.FormaPagamentoListView;
using CadFornecedorListView = PDV.Views.Cadastros.FornecedorListView;
using CadProdutoListView = PDV.Views.Cadastros.ProdutoListView;
using CadUnidadeListView = PDV.Views.Cadastros.UnidadeListView;
using ImportarNovoProdutoView = PDV.Views.ImportarProduto.NovoProdutoView;
using ProdutoInserindoView = PDV.Views.NovoProduto.ProdutoInserindoView;

namespace PDV.Services.Diagnostics
{
    public static class PdvFlowSmokeRunner
    {
        public static void Run(IServiceProvider provider, string runRoot)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            var screenshotsRoot = Path.Combine(runRoot, "screenshots");
            Directory.CreateDirectory(screenshotsRoot);

            var report = new List<string>();
            var progressPath = Path.Combine(runRoot, "progress.log");

            void Log(string message) =>
                File.AppendAllText(progressPath, $"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");

            Log("Fluxo operacional...");
            RunOperationalFlow(provider, report, Log);

            Log("Navegacao...");
            var screenCatalog = CreateScreenCatalog();
            ValidateNavigation(provider, screenCatalog, report, Log);

            Log("Captura de telas...");
            CaptureScreens(provider, screenCatalog, screenshotsRoot, report, Log);

            report.Add($"Screenshots: {screenshotsRoot}");
            File.WriteAllLines(Path.Combine(runRoot, "relatorio-smoke.txt"), report);
            Log("Finalizado OK.");
        }

        private static void RunOperationalFlow(IServiceProvider provider, List<string> report, Action<string> log)
        {
            using var scope = provider.CreateScope();
            AutenticarRetaguardaSeConfigurado(scope.ServiceProvider, report, log);
            var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
            var pdv = scope.ServiceProvider.GetRequiredService<IPdvOperationService>();

            var produto = EnsureProduto(context);
            log("Produto garantido.");

            TestarLocalizacaoEstoque(context, produto, report, log);

            var movimento = pdv.AbrirMovimento(100m);
            Require(movimento.Id.HasValue, "Abertura de movimento nao gerou Id.");
            Require(movimento.DataFechamento == null, "Movimento abriu fechado.");
            report.Add($"Abertura OK: movimento #{movimento.Id} com fundo de troco.");
            log("Abertura OK.");

            pdv.RegistrarSuprimento(50m, "Smoke test");
            Require(context.PdvSuprimentos.Any(s => s.IdPdvMovimento == movimento.Id), "Suprimento nao foi gravado.");
            report.Add("Suprimento OK: gravado no movimento aberto.");
            log("Suprimento OK.");

            pdv.RegistrarSangria(10m, "Smoke test");
            Require(context.PdvSangrias.Any(s => s.IdPdvMovimento == movimento.Id), "Sangria nao foi gravada.");
            report.Add("Sangria OK: gravada no movimento aberto.");
            log("Sangria OK.");

            var valorVenda = Convert.ToDecimal(produto.ValorVenda ?? 12.5);
            var venda = pdv.FinalizarVenda(new[]
            {
                new VendaItemDto(
                    produto.Id,
                    produto.Gtin ?? string.Empty,
                    produto.DescricaoPdv ?? produto.Nome ?? "Produto smoke",
                    1m,
                    valorVenda,
                    0m)
            }, valorVenda + 5m);

            Require(venda.Id.HasValue, "Venda nao gerou Id.");
            Require(context.PdvVendasDetalhe.Any(d => d.IdPdvVendaCabecalho == venda.Id), "Item da venda nao foi gravado.");
            Require(context.ContasReceber.Any(c => c.IdPdvVendaCabecalho == venda.Id), "Conta a receber da venda nao foi gravada.");
            report.Add($"Venda OK: cabecalho #{venda.Id}, item e contas a receber gravados.");
            log("Venda OK.");

            var compra = pdv.SalvarPedidoCompra(new CompraPedidoInput(
                null,
                EnsureFornecedor(context).Nome ?? "Fornecedor Smoke",
                "Smoke",
                DateTime.Today,
                DateTime.Today.AddDays(2),
                DateTime.Today.AddDays(30),
                "30 dias",
                null,
                null,
                "Loja",
                "Loja",
                $"SMK-{DateTime.Now:HHmmss}",
                100m,
                0m,
                0m,
                100m,
                1,
                30,
                DateTime.Today.AddDays(30),
                null));

            Require(compra.Id.HasValue, "Pedido de compra nao gerou Id.");
            Require(context.ContasPagar.Any(c => c.IdCompraPedidoCabecalho == compra.Id), "Conta a pagar da compra nao foi gravada.");
            report.Add($"Compra OK: pedido #{compra.Id} e contas a pagar gravados.");
            log("Compra OK.");

            var fechado = pdv.FecharMovimento(venda.ValorFinal + 50m - 10m, "Dinheiro");
            Require(fechado.DataFechamento.HasValue && fechado.StatusMovimento == "F", "Movimento nao ficou fechado.");
            Require(context.PdvFechamentos.Any(f => f.IdPdvMovimento == fechado.Id), "Fechamento nao foi gravado.");
            report.Add($"Fechamento OK: movimento #{fechado.Id} fechado com registro de fechamento.");
            log("Fechamento OK.");

            var novoMovimento = pdv.AbrirMovimento(0);
            Require(novoMovimento.Id != fechado.Id && novoMovimento.DataFechamento == null, "Nova abertura apos fechamento falhou.");
            report.Add($"Reabertura OK: novo movimento #{novoMovimento.Id} aberto apos fechamento.");
            log("Reabertura OK.");

            _ = pdv.ListarVendas();
            _ = pdv.ListarEstoque();
            _ = pdv.ListarContasPagar();
            _ = pdv.ListarContasReceber();
            _ = pdv.ListarCompras();
            report.Add("Consultas OK: vendas, estoque, contas a pagar, contas a receber e compras carregaram.");
            log("Consultas OK.");

            var featureService = scope.ServiceProvider.GetRequiredService<IPdvFeatureService>();
            var modulos = new[]
            {
                "CadastrosPlus", "MovimentoPlus", "PdvPlus", "Fiscal", "Tributacao",
                "Food", "Delivery", "Relatorios", "ConfiguracoesPlus"
            };

            var totalTelas = 0;
            foreach (var modulo in modulos)
            {
                var telas = featureService.ListarTelas(modulo);
                Require(telas.Count > 0, $"Modulo {modulo} sem telas.");
                totalTelas += telas.Count;

                foreach (var tela in telas)
                {
                    log($"PDV tela {tela.Key}...");
                    _ = featureService.CarregarLinhas(tela.Key);
                }
            }

            report.Add($"PDV OK: {totalTelas} telas complementares carregaram sem erro.");
            log("PDV OK.");

            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PDV_RETAGUARDA_URL")))
            {
                var sync = scope.ServiceProvider.GetRequiredService<IRetaguardaSyncCoordinator>().SincronizarAgoraAsync("smoke-flow").GetAwaiter().GetResult();
                Require(sync.Sincronizado, $"Sincronizacao com retaguarda falhou: {sync.Mensagem}");
                report.Add($"Retaguarda OK: {sync.TotalRegistros} registros de {sync.TotalTabelas} tabelas salvos em {sync.BancoOperacional}.");
                log("Retaguarda OK.");
            }
            else
            {
                report.Add("Retaguarda PULADA: PDV_RETAGUARDA_URL nao configurado.");
                log("Retaguarda PULADA (sem PDV_RETAGUARDA_URL).");
            }
        }

        private static void AutenticarRetaguardaSeConfigurado(IServiceProvider provider, List<string> report, Action<string> log)
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PDV_RETAGUARDA_URL")))
                return;

            var usuario = Environment.GetEnvironmentVariable("PDV_SMOKE_AUTH_USER") ?? "11222333000181|admin";
            var senha = Environment.GetEnvironmentVariable("PDV_SMOKE_AUTH_PASSWORD") ?? "admin123";
            var auth = provider.GetRequiredService<IAuthenticationService>();
            var autenticado = auth.AuthenticateAsync(usuario, senha).GetAwaiter().GetResult();
            Require(autenticado, "Login na retaguarda falhou antes do smoke operacional.");
            if (auth.CurrentSession == null)
                throw new InvalidOperationException("Sessao da retaguarda nao foi criada.");

            provider.GetRequiredService<ILocalTenantService>().GarantirTenantLocal(auth.CurrentSession);
            report.Add("Retaguarda Auth OK: sessao criada para sincronizacao operacional.");
            log("Retaguarda Auth OK.");
        }

        private static void ValidateNavigation(
            IServiceProvider provider,
            IReadOnlyDictionary<string, Func<UserControl>> screenCatalog,
            List<string> report,
            Action<string> log)
        {
            using var scope = provider.CreateScope();
            var navigation = scope.ServiceProvider.GetRequiredService<IViewModelNavigationService>();

            foreach (var key in screenCatalog.Keys)
            {
                navigation.NavigateTo(key);
                Require(navigation.CurrentViewModel != null, $"Rota {key} nao gerou ViewModel.");
                log($"Rota OK: {key}");
            }

            report.Add($"Navegacao OK: {screenCatalog.Count} rotas resolvidas.");
        }

        private static void CaptureScreens(
            IServiceProvider provider,
            IReadOnlyDictionary<string, Func<UserControl>> screenCatalog,
            string screenshotsRoot,
            List<string> report,
            Action<string> log)
        {
            var login = provider.GetRequiredService<LoginView>();
            login.DataContext = provider.GetRequiredService<LoginViewModel>();
            RenderScreen("00-Login", login, screenshotsRoot);
            log("Print OK: Login");

            var auth = provider.GetRequiredService<IAuthenticationService>();
            var alerts = provider.GetRequiredService<IAlertService>();
            var esqueceuSenha = new EsqueceuSenhaView(auth, alerts);
            RenderScreen("01-EsqueceuSenha", esqueceuSenha, screenshotsRoot);
            log("Print OK: EsqueceuSenha");

            var cadastroInicial = new CadastroInicialView(auth, alerts);
            RenderScreen("02-CadastroInicial", cadastroInicial, screenshotsRoot);
            log("Print OK: CadastroInicial");

            var confirmacaoRegistro = new ConfirmacaoRegistroView(auth, alerts, "11222333000181", "admin");
            RenderScreen("03-ConfirmacaoRegistro", confirmacaoRegistro, screenshotsRoot);
            log("Print OK: ConfirmacaoRegistro");

            using var scope = provider.CreateScope();
            var navigation = scope.ServiceProvider.GetRequiredService<IViewModelNavigationService>();
            var shell = new MainView();

            var index = 4;
            foreach (var screen in screenCatalog)
            {
                navigation.NavigateTo(screen.Key);
                RenderScreen($"{index:00}-{screen.Key}", shell, screenshotsRoot);
                log($"Print OK: {screen.Key}");
                index++;
            }

            report.Add($"Prints OK: {index} imagens salvas em {screenshotsRoot}.");
        }

        private static void TestarLocalizacaoEstoque(PdvContext context, Produto produto, List<string> report, Action<string> log)
        {
            var localizacaoJson = JsonSerializer.Serialize(new
            {
                deposito = "DEP-A",
                corredor = "01",
                prateleira = "02",
                nivel = "03",
                posicao = "04",
                enderecoCompleto = "01-02-03-04",
                observacaoArmazenagem = "Smoke test",
                pontoReposicao = (double?)25.0,
                estoqueReservado = (double?)10.0,
                tipoArmazenagem = "Seco",
                observacoes = "OK"
            });

            produto.Localizacao = localizacaoJson;
            context.Produtos.Update(produto);
            context.SaveChanges();

            context.Entry(produto).Reload();
            Require(!string.IsNullOrWhiteSpace(produto.Localizacao), "Localizacao nao foi gravada.");

            using var doc = JsonDocument.Parse(produto.Localizacao!);
            var root = doc.RootElement;
            Require(root.GetProperty("deposito").GetString() == "DEP-A", "Localizacao.deposito incorreto apos gravar.");
            Require(root.GetProperty("corredor").GetString() == "01", "Localizacao.corredor incorreto.");
            Require(root.GetProperty("enderecoCompleto").GetString() == "01-02-03-04", "Localizacao.enderecoCompleto incorreto.");
            Require(root.GetProperty("pontoReposicao").GetDouble() == 25.0, "Localizacao.pontoReposicao incorreto.");

            produto.Localizacao = null;
            context.Produtos.Update(produto);
            context.SaveChanges();

            context.Entry(produto).Reload();
            Require(produto.Localizacao == null, "Localizacao nao foi limpa.");

            report.Add("Localizacao OK: JSON gravado, lido e limpo com sucesso.");
            log("Localizacao OK.");
        }

        private static Produto EnsureProduto(PdvContext context)
        {
            var produto = context.Produtos.FirstOrDefault(p => (p.ValorVenda ?? 0) > 0 && (p.QuantidadeEstoque ?? 0) > 1);
            if (produto != null)
                return produto;

            produto = new Produto
            {
                Id = null,
                Gtin = "7890000000000",
                CodigoInterno = "SMOKE",
                Nome = "Produto Smoke",
                Descricao = "Produto Smoke",
                DescricaoPdv = "Produto Smoke",
                ValorCompra = 8.0,
                ValorVenda = 12.5,
                QuantidadeEstoque = 100,
                EstoqueMinimo = 1,
                EstoqueMaximo = 999,
                Situacao = "Ativo"
            };

            context.Produtos.Add(produto);
            context.SaveChanges();
            return produto;
        }

        private static Fornecedor EnsureFornecedor(PdvContext context)
        {
            var fornecedor = context.Fornecedores.FirstOrDefault();
            if (fornecedor != null)
                return fornecedor;

            fornecedor = new Fornecedor
            {
                Id = null,
                Nome = "Fornecedor Smoke",
                Fantasia = "Fornecedor Smoke",
                CpfCnpj = "00000000000191",
                DataCadastro = DateTime.Today
            };

            context.Fornecedores.Add(fornecedor);
            context.SaveChanges();
            return fornecedor;
        }

        private static void RenderScreen(string name, FrameworkElement view, string screenshotsRoot)
        {
            if (view is Window window)
            {
                if (window.Content is FrameworkElement content)
                {
                    content.DataContext = window.DataContext;
                    window.Content = null;
                    RenderScreen(name, content, screenshotsRoot);
                }

                window.Close();
                return;
            }

            const int width = 1366;
            const int height = 768;

            var host = new Border
            {
                Width = width,
                Height = height,
                Background = Brushes.White,
                Child = view
            };

            try
            {
                host.Measure(new Size(width, height));
                host.Arrange(new Rect(0, 0, width, height));
                host.UpdateLayout();

                var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(host);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using var stream = File.Create(Path.Combine(screenshotsRoot, $"{SanitizeFileName(name)}.png"));
                encoder.Save(stream);
            }
            finally
            {
                host.Child = null;
            }
        }

        private static string SanitizeFileName(string value)
        {
            foreach (var invalid in Path.GetInvalidFileNameChars())
                value = value.Replace(invalid, '-');
            return value;
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        private static IReadOnlyDictionary<string, Func<UserControl>> CreateScreenCatalog()
        {
            return new Dictionary<string, Func<UserControl>>
            {
                ["Home"] = () => new HomeView(),
                ["AberturaCaixa"] = () => new AberturaCaixaView(),
                ["Movimento"] = () => new MovimentoView(),
                ["MovimentoPlus"] = () => new PdvFeatureView(),
                ["Suprimento"] = () => new SuprimentoView(),
                ["Sangria"] = () => new SangriaView(),
                ["Compras"] = () => new ComprasView(),
                ["ComprasForm"] = () => new ComprasFormView(),
                ["Estoque"] = () => new EstoqueView(),
                ["Financeiro"] = () => new FinanceiroView(),
                ["ContasPagar"] = () => new ContasPagarView(),
                ["ContasReceber"] = () => new ContasReceberView(),
                ["Dashboard"] = () => new DashboardView(),
                ["Vendas"] = () => new VendasView(),
                ["Configuracoes"] = () => new ConfiguracoesView(),
                ["CadastrosPlus"] = () => new PdvFeatureView(),
                ["PdvPlus"] = () => new PdvFeatureView(),
                ["Fiscal"] = () => new PdvFeatureView(),
                ["Tributacao"] = () => new PdvFeatureView(),
                ["Food"] = () => new PdvFeatureView(),
                ["Delivery"] = () => new PdvFeatureView(),
                ["Relatorios"] = () => new PdvFeatureView(),
                ["ConfiguracoesPlus"] = () => new PdvFeatureView(),
                ["Ajuda"] = () => new AjudaView(),
                ["Contato"] = () => new ContatoView(),
                ["Sair"] = () => new SairView(),
                ["Cadastros"] = () => new CadastrosPanelUserControl(),
                ["ClienteList"] = () => new CadClienteListView(),
                ["FornecedorList"] = () => new CadFornecedorListView(),
                ["ColaboradorList"] = () => new CadColaboradorListView(),
                ["EmpresaList"] = () => new CadEmpresaListView(),
                ["FormaPagamentoList"] = () => new CadFormaPagamentoListView(),
                ["UnidadeList"] = () => new CadUnidadeListView(),
                ["ProdutoList"] = () => new CadProdutoListView(),
                ["ClienteForm"] = () => new ClienteFormView(),
                ["FornecedorForm"] = () => new FornecedorFormView(),
                ["ColaboradorForm"] = () => new ColaboradorFormView(),
                ["EmpresaForm"] = () => new EmpresaFormView(),
                ["FormaPagamentoForm"] = () => new FormaPagamentoFormView(),
                ["UnidadeForm"] = () => new UnidadeFormView(),
                ["ProdutoForm"] = () => new ProdutoFormView(),
                ["ImportarProduto"] = () => new ImportarProdutoView(),
                ["NovoProduto"] = () => new ImportarNovoProdutoView(),
                ["ProdutoInserindo"] = () => new ProdutoInserindoView()
            };
        }
    }
}

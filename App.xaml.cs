using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDV.Services;
using PDV.Services.Diagnostics;
using PDV.Services.Interfaces;
using PDV.Utilities;
using PDV.ViewModels;
using PDV.Views;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace PDV
{
    public partial class App : Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }
        public static IViewModelNavigationService? NavigationService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            PdvCulture.Configure();
            base.OnStartup(e);

            var smokeMode = e.Args.Any(a => a.Equals("--smoke-flow", StringComparison.OrdinalIgnoreCase));
            var authSmokeMode = e.Args.Any(a => a.Equals("--smoke-auth", StringComparison.OrdinalIgnoreCase));
            var uiProofMode = e.Args.Any(a => a.Equals("--proof-pdv-ui", StringComparison.OrdinalIgnoreCase));
            var tenantGuardSmokeMode = e.Args.Any(a => a.Equals("--smoke-tenant-guard", StringComparison.OrdinalIgnoreCase));
            var cadastroCrudSmokeMode = e.Args.Any(a => a.Equals("--smoke-cadastro-crud", StringComparison.OrdinalIgnoreCase));
            var caixaOperationalSmokeMode = e.Args.Any(a => a.Equals("--smoke-caixa-operacional", StringComparison.OrdinalIgnoreCase));
            var fiscalNfceSmokeMode = e.Args.Any(a => a.Equals("--smoke-fiscal-nfce", StringComparison.OrdinalIgnoreCase));
            var updateSmokeMode = e.Args.Any(a => a.Equals("--smoke-update", StringComparison.OrdinalIgnoreCase));
            var smokeRunRoot = string.Empty;
            var databasePath = smokeMode
                ? PrepareSmokeDatabase(out smokeRunRoot)
                : tenantGuardSmokeMode
                    ? PrepareEmptySmokeDatabase("tenant-guard-smoke", out smokeRunRoot)
                    : cadastroCrudSmokeMode
                        ? PrepareEmptySmokeDatabase("cadastro-crud-smoke", out smokeRunRoot)
                        : caixaOperationalSmokeMode
                            ? PrepareEmptySmokeDatabase("caixa-operacional-smoke", out smokeRunRoot)
                            : fiscalNfceSmokeMode
                                ? PrepareEmptySmokeDatabase("fiscal-nfce-smoke", out smokeRunRoot)
                                : updateSmokeMode
                                    ? PrepareEmptySmokeDatabase("updater-smoke", out smokeRunRoot)
                                    : ResolveDatabasePath();

            var services = new ServiceCollection();
            ConfigureServices(services, databasePath);
            ServiceProvider = services.BuildServiceProvider();
            InitializeDatabase(ServiceProvider);

            if (smokeMode)
            {
                var logPath = Path.Combine(smokeRunRoot, "bootstrap.log");
                File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} ServiceProvider pronto.{Environment.NewLine}");
                try
                {
                    PdvFlowSmokeRunner.Run(ServiceProvider, smokeRunRoot);
                    File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} Smoke runner finalizado.{Environment.NewLine}");
                    Shutdown(0);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} FALHA: {ex.Message}{Environment.NewLine}");
                    Console.Error.WriteLine($"[smoke-flow] FALHA: {ex.Message}");
                    Shutdown(1);
                }
                return;
            }

            if (tenantGuardSmokeMode)
            {
                var exitCode = TenantGuardSmokeRunner.Run(ServiceProvider, smokeRunRoot);
                Shutdown(exitCode);
                return;
            }

            if (authSmokeMode)
            {
                var exitCode = RetaguardaAuthSmokeRunner.Run(ServiceProvider, e.Args);
                Shutdown(exitCode);
                return;
            }

            if (cadastroCrudSmokeMode)
            {
                var exitCode = CadastroCrudSmokeRunner.Run(ServiceProvider, smokeRunRoot);
                Shutdown(exitCode);
                return;
            }

            if (caixaOperationalSmokeMode)
            {
                var exitCode = CaixaOperationalSmokeRunner.Run(ServiceProvider, smokeRunRoot);
                Shutdown(exitCode);
                return;
            }

            if (fiscalNfceSmokeMode)
            {
                var exitCode = FiscalNfceSmokeRunner.Run(ServiceProvider, smokeRunRoot);
                Shutdown(exitCode);
                return;
            }

            if (updateSmokeMode)
            {
                var exitCode = UpdaterSmokeRunner.Run(smokeRunRoot);
                Shutdown(exitCode);
                return;
            }

            if (uiProofMode)
            {
                var exitCode = PdvUiProofRunner.Run(ServiceProvider);
                Shutdown(exitCode);
                return;
            }

            NavigationService = ServiceProvider.GetService<IViewModelNavigationService>();
            ServiceProvider.GetRequiredService<IRetaguardaSyncCoordinator>().Start();
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }

        private void ConfigureServices(ServiceCollection services, string databasePath)
        {
            services.AddSingleton<ILocalDatabaseService>(_ => new LocalDatabaseService(databasePath));
            services.AddDbContext<PdvContext>((provider, options) =>
                options.UseSqlite($"Data Source={provider.GetRequiredService<ILocalDatabaseService>().CurrentDatabasePath}"),
                contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Transient
            );

            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IAlertService, AlertService>();
            services.AddSingleton<IViewModelNavigationService, ViewModelNavigationService>();
            services.AddSingleton<PdvCashSessionState>();
            services.AddTransient<IPdvOperationService, PdvOperationService>();
            services.AddTransient<IProdutoImagemService, ProdutoImagemService>();
            services.AddTransient<IProdutoImagemSyncService, ProdutoImagemSyncService>();
            services.AddTransient<IFiscalNfceService, FiscalNfceService>();
            services.AddTransient<IPdvFeatureService, PdvFeatureService>();
            services.AddTransient<IRetaguardaSyncService, RetaguardaSyncService>();
            services.AddTransient<ILocalTenantService, LocalTenantService>();
            services.AddSingleton<IRetaguardaSyncCoordinator, RetaguardaSyncCoordinator>();
            services.AddSingleton<IPdvUpdateLauncher, PdvUpdateLauncher>();

            // Cadastros - Listas
            services.AddTransient<PDV.ViewModels.Cadastros.ClienteListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.FornecedorListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.ColaboradorListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.EmpresaListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.FormaPagamentoListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.UnidadeListViewModel>();
            services.AddTransient<PDV.ViewModels.Cadastros.ProdutoListViewModel>();

            // Cadastros - Formul�rios
            services.AddTransient<ClienteFormViewModel>();
            services.AddTransient<FornecedorFormViewModel>();
            services.AddTransient<ColaboradorFormViewModel>();
            services.AddTransient<EmpresaFormViewModel>();
            services.AddTransient<FormaPagamentoFormViewModel>();
            services.AddTransient<UnidadeFormViewModel>();
            services.AddTransient<ProdutoFormViewModel>();

            // Geral
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<CadastrosPanelViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<AberturaCaixaViewModel>();
            services.AddTransient<MovimentoViewModel>();
            services.AddTransient<MovimentoPlusViewModel>();
            services.AddTransient<SuprimentoViewModel>();
            services.AddTransient<SangriaViewModel>();
            services.AddTransient<ComprasViewModel>();
            services.AddTransient<ComprasFormViewModel>();
            services.AddTransient<EstoqueViewModel>();
            services.AddTransient<FinanceiroViewModel>();
            services.AddTransient<ContasPagarViewModel>();
            services.AddTransient<ContasReceberViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<VendasViewModel>();
            services.AddTransient<ConfiguracoesViewModel>();
            services.AddTransient<CadastrosPlusViewModel>();
            services.AddTransient<PdvPlusViewModel>();
            services.AddTransient<FiscalViewModel>();
            services.AddTransient<TributacaoViewModel>();
            services.AddTransient<FoodViewModel>();
            services.AddTransient<DeliveryViewModel>();
            services.AddTransient<RelatoriosViewModel>();
            services.AddTransient<ConfiguracoesPlusViewModel>();
            services.AddTransient<AjudaViewModel>();
            services.AddTransient<ContatoViewModel>();
            services.AddTransient<SairViewModel>();

            // Importar / Novo Produto
            services.AddTransient<ImportarProdutoViewModel>();
            services.AddTransient<NovoProdutoViewModel>();

            // Produto Inserindo 
            services.AddTransient<PDV.ViewModels.NovoProduto.ProdutoInserindoViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginView>();
        }

        private static string ResolveDatabasePath()
        {
            var configuredPath = Environment.GetEnvironmentVariable("PDV_DATABASE_PATH");
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(configuredPath)) ?? AppContext.BaseDirectory);
                return Path.GetFullPath(configuredPath);
            }

            var repoRoot = FindRepoRoot();
            if (File.Exists(Path.Combine(repoRoot, "PDV.csproj")))
                return Path.Combine(repoRoot, "pdv.sqlite");

            PdvRuntimePaths.EnsureDataDirectory();
            var installedDatabasePath = PdvRuntimePaths.LoginDatabasePath;
            MigrarDadosLegadosSeNecessario();
            SeedInstalledDatabaseIfNeeded(installedDatabasePath);
            return installedDatabasePath;
        }

        private static void MigrarDadosLegadosSeNecessario()
        {
            var destino = PdvRuntimePaths.DataDirectory;

            // Ja tem dados no novo path — nada a migrar
            if (Directory.Exists(destino) && Directory.GetFileSystemEntries(destino).Length > 0)
                return;

            var legadoBase = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "TechOne", "PDV");

            if (!Directory.Exists(legadoBase))
                return;

            try
            {
                CopiarDiretorioRecursivo(legadoBase, destino);
            }
            catch
            {
                // Falha silenciosa: o restore do servidor cobre o caso
            }
        }

        private static void CopiarDiretorioRecursivo(string origem, string destino)
        {
            Directory.CreateDirectory(destino);

            foreach (var arquivo in Directory.GetFiles(origem))
            {
                var nomeArquivo = Path.GetFileName(arquivo);
                var destinoArquivo = Path.Combine(destino, nomeArquivo);
                if (!File.Exists(destinoArquivo))
                    File.Copy(arquivo, destinoArquivo);
            }

            foreach (var subdir in Directory.GetDirectories(origem))
            {
                var nomeSubdir = Path.GetFileName(subdir);
                CopiarDiretorioRecursivo(subdir, Path.Combine(destino, nomeSubdir));
            }
        }

        private static void SeedInstalledDatabaseIfNeeded(string installedDatabasePath)
        {
            if (File.Exists(installedDatabasePath))
                return;

            var legacyDatabasePath = Path.Combine(AppContext.BaseDirectory, "pdv.sqlite");
            if (!File.Exists(legacyDatabasePath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(installedDatabasePath) ?? PdvRuntimePaths.DataDirectory);
            File.Copy(legacyDatabasePath, installedDatabasePath, overwrite: false);
        }

        private static void InitializeDatabase(ServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
            context.Database.Migrate();
        }

        private static string PrepareSmokeDatabase(out string runRoot)
        {
            return PrepareEmptySmokeDatabase("pdv-flow-smoke", out runRoot);
        }

        private static string PrepareEmptySmokeDatabase(string name, out string runRoot)
        {
            var repoRoot = FindRepoRoot();
            var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            runRoot = Path.Combine(repoRoot, "artifacts", name, stamp);
            Directory.CreateDirectory(runRoot);
            return Path.Combine(runRoot, "pdv-test.sqlite");
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "PDV.csproj")))
                    return dir.FullName;
                dir = dir.Parent;
            }

            return AppContext.BaseDirectory;
        }
    }
}

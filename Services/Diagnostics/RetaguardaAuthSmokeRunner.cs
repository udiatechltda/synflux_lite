using PDV.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDV.Services.Diagnostics
{
    public static class RetaguardaAuthSmokeRunner
    {
        public static int Run(IServiceProvider serviceProvider, string[] args)
        {
            var runRoot = PrepararDiretorio();
            var logPath = Path.Combine(runRoot, "result.log");

            try
            {
                var usuario = LerParametro(args, "--auth-user")
                    ?? Environment.GetEnvironmentVariable("PDV_SMOKE_AUTH_USER")
                    ?? "11222333000181|admin";

                var senha = LerParametro(args, "--auth-password")
                    ?? Environment.GetEnvironmentVariable("PDV_SMOKE_AUTH_PASSWORD")
                    ?? "admin123";

                var authService = serviceProvider.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
                if (authService == null)
                    throw new InvalidOperationException("IAuthenticationService nao foi registrado.");

                File.AppendAllText(logPath, $"URL={Environment.GetEnvironmentVariable("PDV_RETAGUARDA_URL") ?? "http://localhost:5000"}{Environment.NewLine}");
                File.AppendAllText(logPath, $"Usuario={usuario}{Environment.NewLine}");

                var autenticado = Task.Run(() => authService.AuthenticateAsync(usuario, senha)).GetAwaiter().GetResult();
                File.AppendAllText(logPath, $"Autenticado={autenticado}{Environment.NewLine}");

                if (!autenticado)
                    return 1;

                if (authService is AuthenticationService service && service.CurrentSession != null)
                {
                    File.AppendAllText(logPath, $"Empresa={service.CurrentSession.Empresa.Cnpj} - {service.CurrentSession.Empresa.NomeFantasia}{Environment.NewLine}");
                    File.AppendAllText(logPath, $"Perfil={service.CurrentSession.Usuario.Perfil}{Environment.NewLine}");
                    File.AppendAllText(logPath, $"UsuarioConfirmado={service.CurrentSession.Usuario.Confirmado}{Environment.NewLine}");
                    File.AppendAllText(logPath, $"EmpresaRegistrada={service.CurrentSession.Empresa.Registrado}{Environment.NewLine}");
                    File.AppendAllText(logPath, $"TokenLength={service.CurrentSession.Token.Length}{Environment.NewLine}");
                    File.AppendAllText(logPath, $"ExpiraEm={service.CurrentSession.ExpiraEm:O}{Environment.NewLine}");

                    if (string.IsNullOrWhiteSpace(service.CurrentSession.Token))
                    {
                        File.AppendAllText(logPath, $"Erro=Login pendente de confirmacao nao retornou token.{Environment.NewLine}");
                        return 1;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"Erro={ex.Message}{Environment.NewLine}{ex}{Environment.NewLine}");
                return 1;
            }
        }

        private static string? LerParametro(string[] args, string nome)
        {
            var index = Array.FindIndex(args, a => a.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && index + 1 < args.Length)
                return args[index + 1];

            var prefixo = nome + "=";
            var inline = args.FirstOrDefault(a => a.StartsWith(prefixo, StringComparison.OrdinalIgnoreCase));
            return inline?.Substring(prefixo.Length);
        }

        private static string PrepararDiretorio()
        {
            var repoRoot = EncontrarRepoRoot();
            var runRoot = Path.Combine(repoRoot, "artifacts", "retaguarda-auth-smoke", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            Directory.CreateDirectory(runRoot);
            return runRoot;
        }

        private static string EncontrarRepoRoot()
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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.IO;

namespace PDV.Services.Diagnostics
{
    public static class TenantGuardSmokeRunner
    {
        public static int Run(IServiceProvider provider, string runRoot)
        {
            var logPath = Path.Combine(runRoot, "tenant-guard.log");

            void Log(string message) =>
                File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");

            try
            {
                using var scope = provider.CreateScope();
                var tenantService = scope.ServiceProvider.GetRequiredService<ILocalTenantService>();
                var databaseService = scope.ServiceProvider.GetRequiredService<ILocalDatabaseService>();

                var empresaA = NovaSessao(1, "77000000000001");
                var empresaB = NovaSessao(2, "77000000000002");

                tenantService.GarantirTenantLocal(empresaA);
                var dbEmpresaA = databaseService.CurrentDatabasePath;
                Log("Tenant A vinculado na base local propria.");

                using var scopeA = provider.CreateScope();
                var context = scopeA.ServiceProvider.GetRequiredService<PdvContext>();
                context.Database.ExecuteSqlRaw("INSERT INTO CLIENTE (NOME) VALUES ('CLI-TENANT-A-BLOQUEIO')");
                Log("Dado local da empresa A criado.");

                tenantService.GarantirTenantLocal(empresaB);
                var dbEmpresaB = databaseService.CurrentDatabasePath;
                if (string.Equals(dbEmpresaA, dbEmpresaB, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Falha critica: empresas diferentes usaram o mesmo SQLite local.");

                using var scopeB = provider.CreateScope();
                var contextB = scopeB.ServiceProvider.GetRequiredService<PdvContext>();
                contextB.Database.ExecuteSqlRaw("INSERT INTO CLIENTE (NOME) VALUES ('CLI-TENANT-B-ISOLADO')");
                Log("Tenant B usou outra base local sem incomodar o usuario.");

                var clienteAEmB = contextB.Database.SqlQueryRaw<int>(
                    "SELECT COUNT(*) AS Value FROM CLIENTE WHERE NOME = 'CLI-TENANT-A-BLOQUEIO'").ToListAsync().GetAwaiter().GetResult()[0];
                if (clienteAEmB != 0)
                    throw new InvalidOperationException("Dado local da empresa A apareceu na base da empresa B.");

                var tenantTableCount = contextB.Database.SqlQueryRaw<int>(
                    "SELECT COUNT(*) AS Value FROM sqlite_master WHERE type = 'table' AND name = 'PDV_TENANT_LOCAL'").ToListAsync().GetAwaiter().GetResult()[0];
                if (tenantTableCount != 1)
                    throw new InvalidOperationException("Tabela PDV_TENANT_LOCAL nao foi criada.");

                Log("Smoke tenant guard concluido com sucesso: bases locais separadas por empresa.");
                return 0;
            }
            catch (Exception ex)
            {
                Log($"ERRO: {ex.Message}");
                Log(ex.ToString());
                return 1;
            }
        }

        private static RetaguardaAuthSession NovaSessao(int idEmpresa, string cnpj)
        {
            return new RetaguardaAuthSession
            {
                Token = "teste",
                ExpiraEm = DateTime.UtcNow.AddHours(1),
                Usuario = new RetaguardaUsuarioResponse
                {
                    Id = idEmpresa,
                    Login = "admin",
                    Confirmado = "S",
                    Perfil = "Administrador"
                },
                Empresa = new RetaguardaEmpresaResponse
                {
                    Id = idEmpresa,
                    Cnpj = cnpj,
                    Registrado = "S",
                    BancoOperacional = "pdv_operacional"
                }
            };
        }
    }
}

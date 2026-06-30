using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace PDV.Services
{
    public class LocalTenantService : ILocalTenantService
    {
        private const string TenantTable = "PDV_TENANT_LOCAL";
        private static readonly string[] IgnoredTables =
        {
            "__EFMigrationsHistory",
            TenantTable,
            "sqlite_sequence"
        };

        private readonly ILocalDatabaseService _databaseService;
        private readonly IServiceScopeFactory _scopeFactory;

        public LocalTenantService(ILocalDatabaseService databaseService, IServiceScopeFactory scopeFactory)
        {
            _databaseService = databaseService;
            _scopeFactory = scopeFactory;
        }

        public void GarantirTenantLocal(RetaguardaAuthSession session)
        {
            if (session == null)
                throw new InvalidOperationException("Sessao da retaguarda nao informada.");

            var cnpjSessao = SomenteDigitos(session.Empresa.Cnpj);
            if (string.IsNullOrWhiteSpace(cnpjSessao) || session.Empresa.Id <= 0)
                throw new InvalidOperationException("Sessao da retaguarda sem empresa valida.");

            AdotarBaseLegadaSeAplicavel(session, cnpjSessao);
            _databaseService.UseTenantDatabase(session);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PdvContext>();
            context.Database.Migrate();

            var connection = context.Database.GetDbConnection();
            var deveFechar = connection.State != ConnectionState.Open;
            if (deveFechar)
                connection.Open();

            try
            {
                CriarTabelaTenant(connection);
                var tenant = LerTenant(connection);

                if (tenant != null)
                {
                    var tenantAtual = tenant.Value;
                    if (tenantAtual.IdEmpresa != session.Empresa.Id || tenantAtual.Cnpj != cnpjSessao)
                    {
                        throw new InvalidOperationException(
                            $"Base local interna pertence ao CNPJ {tenantAtual.Cnpj}, mas a sessao atual e do CNPJ {cnpjSessao}.");
                    }

                    return;
                }

                var cnpjEmpresaLocal = LerCnpjEmpresaLocal(connection);
                if (!string.IsNullOrWhiteSpace(cnpjEmpresaLocal) && cnpjEmpresaLocal != cnpjSessao)
                {
                    throw new InvalidOperationException(
                        $"Base local interna contem empresa {cnpjEmpresaLocal}, mas a sessao atual e do CNPJ {cnpjSessao}.");
                }

                if (string.IsNullOrWhiteSpace(cnpjEmpresaLocal) && PossuiDadosOperacionais(connection))
                {
                    throw new InvalidOperationException(
                        "Base local interna ja possui dados sem empresa vinculada. Para evitar mistura multitenant, o acesso foi bloqueado.");
                }

                GravarTenant(connection, session.Empresa.Id, cnpjSessao);
            }
            finally
            {
                if (deveFechar)
                    connection.Close();
            }
        }

        private void AdotarBaseLegadaSeAplicavel(RetaguardaAuthSession session, string cnpjSessao)
        {
            var destino = _databaseService.GetTenantDatabasePath(session);
            if (File.Exists(destino))
                return;

            var origem = _databaseService.LoginDatabasePath;
            if (!File.Exists(origem) || string.Equals(Path.GetFullPath(origem), Path.GetFullPath(destino), StringComparison.OrdinalIgnoreCase))
                return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={origem}");
            connection.Open();

            var tenant = TabelaExiste(connection, TenantTable) ? LerTenant(connection) : null;
            if (tenant != null)
            {
                if (tenant.Value.IdEmpresa != session.Empresa.Id || tenant.Value.Cnpj != cnpjSessao)
                    return;

                CopiarBase(origem, destino);
                return;
            }

            var cnpjEmpresaLocal = LerCnpjEmpresaLocal(connection);
            if (!string.IsNullOrWhiteSpace(cnpjEmpresaLocal) && cnpjEmpresaLocal == cnpjSessao)
            {
                CopiarBase(origem, destino);
                return;
            }
        }

        private static void CopiarBase(string origem, string destino)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destino) ?? AppContext.BaseDirectory);
            File.Copy(origem, destino, overwrite: false);
        }

        private static void CriarTabelaTenant(System.Data.Common.DbConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $@"
                CREATE TABLE IF NOT EXISTS {TenantTable} (
                    ID INTEGER PRIMARY KEY CHECK (ID = 1),
                    ID_EMPRESA INTEGER NOT NULL,
                    CNPJ TEXT NOT NULL,
                    VINCULADO_EM TEXT NOT NULL
                )";
            command.ExecuteNonQuery();
        }

        private static (int IdEmpresa, string Cnpj)? LerTenant(System.Data.Common.DbConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT ID_EMPRESA, CNPJ FROM {TenantTable} WHERE ID = 1 LIMIT 1";

            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            var idEmpresa = Convert.ToInt32(reader["ID_EMPRESA"]);
            var cnpj = SomenteDigitos(reader["CNPJ"]?.ToString() ?? string.Empty);
            return (idEmpresa, cnpj);
        }

        private static string LerCnpjEmpresaLocal(System.Data.Common.DbConnection connection)
        {
            if (!TabelaExiste(connection, "EMPRESA"))
                return string.Empty;

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Cnpj
                  FROM EMPRESA
                 WHERE Cnpj IS NOT NULL
                   AND TRIM(Cnpj) <> ''
                 LIMIT 1";

            var value = command.ExecuteScalar();
            return SomenteDigitos(value?.ToString() ?? string.Empty);
        }

        private static bool PossuiDadosOperacionais(System.Data.Common.DbConnection connection)
        {
            foreach (var tableName in ListarTabelas(connection))
            {
                if (IgnoredTables.Contains(tableName, StringComparer.OrdinalIgnoreCase))
                    continue;

                using var command = connection.CreateCommand();
                command.CommandText = $"SELECT 1 FROM \"{EscaparIdentificadorSqlite(tableName)}\" LIMIT 1";
                if (command.ExecuteScalar() != null)
                    return true;
            }

            return false;
        }

        private static void GravarTenant(System.Data.Common.DbConnection connection, int idEmpresa, string cnpj)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $@"
                INSERT INTO {TenantTable} (ID, ID_EMPRESA, CNPJ, VINCULADO_EM)
                VALUES (1, @idEmpresa, @cnpj, @vinculadoEm)";

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@idEmpresa";
            idParam.Value = idEmpresa;
            command.Parameters.Add(idParam);

            var cnpjParam = command.CreateParameter();
            cnpjParam.ParameterName = "@cnpj";
            cnpjParam.Value = cnpj;
            command.Parameters.Add(cnpjParam);

            var dataParam = command.CreateParameter();
            dataParam.ParameterName = "@vinculadoEm";
            dataParam.Value = DateTime.UtcNow.ToString("O");
            command.Parameters.Add(dataParam);

            command.ExecuteNonQuery();
        }

        private static bool TabelaExiste(System.Data.Common.DbConnection connection, string tableName)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 1
                  FROM sqlite_master
                 WHERE type = 'table'
                   AND name = @tableName
                 LIMIT 1";

            var param = command.CreateParameter();
            param.ParameterName = "@tableName";
            param.Value = tableName;
            command.Parameters.Add(param);

            return command.ExecuteScalar() != null;
        }

        private static string[] ListarTabelas(System.Data.Common.DbConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT name
                  FROM sqlite_master
                 WHERE type = 'table'
                   AND name NOT LIKE 'sqlite_%'
                 ORDER BY name";

            using var reader = command.ExecuteReader();
            var tabelas = new System.Collections.Generic.List<string>();
            while (reader.Read())
                tabelas.Add(reader.GetString(0));

            return tabelas.ToArray();
        }

        private static string EscaparIdentificadorSqlite(string value)
        {
            return (value ?? string.Empty).Replace("\"", "\"\"");
        }

        private static string SomenteDigitos(string value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }
    }
}

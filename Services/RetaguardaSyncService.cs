using Microsoft.EntityFrameworkCore;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PDV.Services
{
    public class RetaguardaSyncService : IRetaguardaSyncService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        private readonly PdvContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalTenantService _localTenantService;
        private readonly IProdutoImagemSyncService _produtoImagemSyncService;

        public RetaguardaSyncService(
            PdvContext context,
            IAuthenticationService authenticationService,
            ILocalTenantService localTenantService,
            IProdutoImagemSyncService produtoImagemSyncService)
        {
            _context = context;
            _authenticationService = authenticationService;
            _localTenantService = localTenantService;
            _produtoImagemSyncService = produtoImagemSyncService;
        }

        public async Task<RetaguardaSyncResult> SincronizarTudoAsync()
        {
            if (_authenticationService is not AuthenticationService auth || auth.CurrentSession == null)
            {
                return new RetaguardaSyncResult
                {
                    Sincronizado = false,
                    Mensagem = "Sem sessao autenticada da retaguarda."
                };
            }

            _localTenantService.GarantirTenantLocal(auth.CurrentSession);
            var snapshot = CriarSnapshot();
            using var httpClient = CriarHttpClient(auth.CurrentSession.Token);
            var response = await httpClient.PostAsJsonAsync("sincroniza/pdv/snapshot", snapshot, JsonOptions).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new InvalidOperationException($"Falha ao sincronizar PDV na retaguarda: {(int)response.StatusCode} {erro}");
            }

            var retorno = await response.Content.ReadFromJsonAsync<PdvSnapshotResponse>(JsonOptions).ConfigureAwait(false)
                ?? new PdvSnapshotResponse();
            var imagensSincronizadas = await _produtoImagemSyncService.SincronizarAsync(httpClient).ConfigureAwait(false);

            return new RetaguardaSyncResult
            {
                Sincronizado = true,
                Mensagem = imagensSincronizadas > 0
                    ? $"Snapshot sincronizado. {imagensSincronizadas} imagem(ns) de produto sincronizada(s)."
                    : "Snapshot sincronizado.",
                BancoOperacional = retorno.BancoOperacional,
                TotalTabelas = retorno.TotalTabelas,
                TotalRegistros = retorno.TotalRegistros
            };
        }

        public async Task<RetaguardaSyncResult> RestaurarDoServidorAsync()
        {
            if (_authenticationService is not AuthenticationService auth || auth.CurrentSession == null)
            {
                return new RetaguardaSyncResult
                {
                    Sincronizado = false,
                    Mensagem = "Sem sessao autenticada da retaguarda."
                };
            }

            _localTenantService.GarantirTenantLocal(auth.CurrentSession);
            using var httpClient = CriarHttpClient(auth.CurrentSession.Token);
            var response = await httpClient.GetAsync("sincroniza/pdv/restore").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new RetaguardaSyncResult
                {
                    Sincronizado = false,
                    Mensagem = $"Falha ao restaurar do servidor: {(int)response.StatusCode} {erro}"
                };
            }

            var retorno = await response.Content.ReadFromJsonAsync<PdvRestoreResponse>(JsonOptions).ConfigureAwait(false);
            if (retorno == null || retorno.Tabelas == null || retorno.Tabelas.Count == 0)
            {
                return new RetaguardaSyncResult
                {
                    Sincronizado = true,
                    Mensagem = "Nenhum dado no servidor para restaurar."
                };
            }

            await AplicarRestoreLocalAsync(retorno).ConfigureAwait(false);

            return new RetaguardaSyncResult
            {
                Sincronizado = true,
                Mensagem = $"Restaurado do servidor: {retorno.TotalTabelas} tabela(s), {retorno.TotalRegistros} registro(s).",
                BancoOperacional = retorno.BancoOperacional,
                TotalTabelas = retorno.TotalTabelas,
                TotalRegistros = retorno.TotalRegistros
            };
        }

        private async Task AplicarRestoreLocalAsync(PdvRestoreResponse retorno)
        {
            var connection = _context.Database.GetDbConnection();
            var deveFechar = connection.State != System.Data.ConnectionState.Open;
            if (deveFechar)
                connection.Open();

            try
            {
                foreach (var tabela in retorno.Tabelas)
                {
                    foreach (var registro in tabela.Registros)
                    {
                        if (string.IsNullOrWhiteSpace(registro.DadosJson))
                            continue;

                        try
                        {
                            await AplicarRegistroSqliteAsync(connection, tabela.Nome, registro.DadosJson).ConfigureAwait(false);
                        }
                        catch
                        {
                            // falha em registro individual nao interrompe o restore
                        }
                    }
                }
            }
            finally
            {
                if (deveFechar)
                    connection.Close();
            }
        }

        private static async Task AplicarRegistroSqliteAsync(System.Data.Common.DbConnection connection, string tableName, string dadosJson)
        {
            using var doc = JsonDocument.Parse(dadosJson);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
                return;

            var colunas = new List<string>();
            var valores = new List<object?>();

            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                colunas.Add(prop.Name);
                valores.Add(ExtrairValorJson(prop.Value));
            }

            if (colunas.Count == 0)
                return;

            var colunasEsc = string.Join(", ", colunas.Select(c => $"\"{EscaparIdentificadorSqlite(c)}\""));
            var placeholders = string.Join(", ", colunas.Select((_, i) => $"@rv{i}"));
            var sql = $"INSERT OR REPLACE INTO \"{EscaparIdentificadorSqlite(tableName)}\" ({colunasEsc}) VALUES ({placeholders})";

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            for (var i = 0; i < colunas.Count; i++)
            {
                var param = command.CreateParameter();
                param.ParameterName = $"@rv{i}";
                param.Value = valores[i] ?? DBNull.Value;
                command.Parameters.Add(param);
            }

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        private static object? ExtrairValorJson(JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.Null or JsonValueKind.Undefined => null,
                JsonValueKind.True => 1,
                JsonValueKind.False => 0,
                JsonValueKind.Number when value.TryGetInt64(out var l) => l,
                JsonValueKind.Number when value.TryGetDouble(out var d) => d,
                JsonValueKind.String => value.GetString(),
                _ => value.GetRawText()
            };
        }

        private PdvSnapshotRequest CriarSnapshot()
        {
            var snapshot = new PdvSnapshotRequest
            {
                DispositivoId = Environment.MachineName
            };

            var connection = _context.Database.GetDbConnection();
            var deveFechar = connection.State != ConnectionState.Open;
            if (deveFechar)
                connection.Open();

            try
            {
                foreach (var tableName in ListarTabelasSqlite(connection))
                {
                    var tabela = new PdvSnapshotTable { Nome = tableName };
                    using var command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM \"{EscaparIdentificadorSqlite(tableName)}\"";
                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var registro = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            registro[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }

                        var dadosJson = JsonSerializer.Serialize(registro, JsonOptions);
                        tabela.Registros.Add(new PdvSnapshotRecord
                        {
                            IdLocal = ObterIdLocal(registro, dadosJson),
                            DadosJson = dadosJson,
                            Hash = Hash(dadosJson)
                        });
                    }

                    snapshot.Tabelas.Add(tabela);
                }
            }
            finally
            {
                if (deveFechar)
                    connection.Close();
            }

            return snapshot;
        }

        private static HttpClient CriarHttpClient(string token)
        {
            var baseUrl = RetaguardaEndpointResolver.ObterBaseUrl();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"),
                Timeout = TimeSpan.FromSeconds(60)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return httpClient;
        }

        private static IReadOnlyList<string> ListarTabelasSqlite(System.Data.Common.DbConnection connection)
        {
            var tabelas = new List<string>();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT name
                  FROM sqlite_master
                 WHERE type = 'table'
                   AND name NOT LIKE 'sqlite_%'
                   AND name <> 'PDV_TENANT_LOCAL'
                 ORDER BY name";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var nome = reader.GetString(0);
                if (!string.IsNullOrWhiteSpace(nome))
                    tabelas.Add(nome);
            }

            return tabelas;
        }

        private static string ObterIdLocal(Dictionary<string, object?> registro, string dadosJson)
        {
            if (registro.TryGetValue("Id", out var id) || registro.TryGetValue("ID", out id))
                return id?.ToString() ?? Hash(dadosJson);

            return Hash(dadosJson);
        }

        private static string EscaparIdentificadorSqlite(string value)
        {
            return (value ?? string.Empty).Replace("\"", "\"\"");
        }

        private static string Hash(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value ?? string.Empty));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}

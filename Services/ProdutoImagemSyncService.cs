using Microsoft.EntityFrameworkCore;
using PDV.Models.Pdv.Cadastros;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PDV.Services
{
    public class ProdutoImagemSyncService : IProdutoImagemSyncService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        private readonly PdvContext _context;
        private readonly ILocalDatabaseService _databaseService;
        private bool _schemaGarantido;

        public ProdutoImagemSyncService(PdvContext context, ILocalDatabaseService databaseService)
        {
            _context = context;
            _databaseService = databaseService;
        }

        public async Task<int> SincronizarAsync(HttpClient httpClient)
        {
            GarantirSchemaProdutoImagem();

            var exclusoes = await EnviarExclusoesPendentesAsync(httpClient).ConfigureAwait(false);
            var manifest = await ObterManifestImagensAsync(httpClient).ConfigureAwait(false);
            var enviadas = await EnviarImagensLocaisParaRetaguardaAsync(httpClient, manifest).ConfigureAwait(false);

            if (exclusoes > 0 || enviadas > 0)
                manifest = await ObterManifestImagensAsync(httpClient).ConfigureAwait(false);

            var baixadas = await BaixarImagensRetaguardaAsync(httpClient, manifest).ConfigureAwait(false);
            return exclusoes + enviadas + baixadas;
        }

        private async Task<int> EnviarExclusoesPendentesAsync(HttpClient httpClient)
        {
            var pendentes = _context.ProdutosImagens
                .Where(i => i.IdProduto.HasValue && i.PendenteExclusao == "S")
                .ToList();

            var removidas = 0;
            foreach (var local in pendentes)
            {
                try
                {
                    using var response = await httpClient
                        .DeleteAsync($"api/produtos/{local.IdProduto!.Value}/imagem")
                        .ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                        continue;

                    _context.ProdutosImagens.Remove(local);
                    removidas++;
                }
                catch
                {
                    // A pendencia permanece para nova tentativa do coordenador.
                }
            }

            if (removidas > 0)
                _context.SaveChanges();

            return removidas;
        }

        private static async Task<ProdutoImagemManifestResponse?> ObterManifestImagensAsync(HttpClient httpClient)
        {
            try
            {
                return await httpClient
                    .GetFromJsonAsync<ProdutoImagemManifestResponse>("api/produtos/imagens/manifest", JsonOptions)
                    .ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        private async Task<int> EnviarImagensLocaisParaRetaguardaAsync(HttpClient httpClient, ProdutoImagemManifestResponse? manifest)
        {
            var remotoPorProduto = (manifest?.Imagens ?? new List<ProdutoImagemManifestItem>())
                .Where(i => i.ProdutoId > 0 && !i.Excluido)
                .GroupBy(i => i.ProdutoId)
                .ToDictionary(g => g.Key, g => g.First());

            var locais = _context.ProdutosImagens
                .Where(i =>
                    i.IdProduto.HasValue &&
                    !string.IsNullOrWhiteSpace(i.Imagem) &&
                    i.PendenteExclusao != "S")
                .ToList();

            var enviadas = 0;
            foreach (var local in locais)
            {
                var produtoId = local.IdProduto!.Value;
                var path = ResolverImagemLocalAbsoluta(local.Imagem);
                if (path == null)
                    continue;

                var hashLocal = CalcularHashArquivo(path);
                if (!string.Equals(local.HashSha256, hashLocal, StringComparison.OrdinalIgnoreCase))
                {
                    local.HashSha256 = hashLocal;
                    local.TamanhoBytes = new FileInfo(path).Length;
                    local.ContentType = ResolverContentType(path);
                    local.AtualizadoEm = DateTime.UtcNow;
                    local.PendenteUpload = "S";
                }

                var uploadObrigatorio = local.PendenteUpload == "S";
                var remotoAtualizado = remotoPorProduto.TryGetValue(produtoId, out var remoto) &&
                    string.Equals(remoto.Hash, hashLocal, StringComparison.OrdinalIgnoreCase);

                if (!uploadObrigatorio && remotoAtualizado)
                {
                    AtualizarMetadadosSincronizados(local, remoto!, path);
                    continue;
                }

                if (!uploadObrigatorio && manifest == null)
                    continue;

                var resposta = await EnviarImagemLocalAsync(httpClient, produtoId, path).ConfigureAwait(false);
                if (resposta == null)
                    continue;

                AtualizarMetadadosUpload(local, resposta, path);
                enviadas++;
            }

            if (enviadas > 0 || _context.ChangeTracker.HasChanges())
                _context.SaveChanges();

            return enviadas;
        }

        private static async Task<ProdutoImagemUploadResponse?> EnviarImagemLocalAsync(HttpClient httpClient, int produtoId, string path)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                form.Add(new StringContent(produtoId.ToString()), "produtoId");

                using var stream = File.OpenRead(path);
                using var arquivo = new StreamContent(stream);
                arquivo.Headers.ContentType = new MediaTypeHeaderValue(ResolverContentType(path));
                form.Add(arquivo, "arquivo", Path.GetFileName(path));

                using var response = await httpClient.PostAsync("api/imagens", form).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content
                    .ReadFromJsonAsync<ProdutoImagemUploadResponse>(JsonOptions)
                    .ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        private async Task<int> BaixarImagensRetaguardaAsync(HttpClient httpClient, ProdutoImagemManifestResponse? manifest)
        {
            if (manifest?.Imagens == null || manifest.Imagens.Count == 0)
                return 0;

            var baixadas = 0;
            foreach (var item in manifest.Imagens.Where(i => i.ProdutoId > 0))
            {
                if (item.Excluido)
                {
                    RemoverImagemLocalSeNaoPendente(item.ProdutoId);
                    continue;
                }

                var extensao = ResolverExtensao(item);
                if (extensao == null)
                    continue;

                var destinoAbsoluto = ObterImagemProdutoPath(item.ProdutoId, extensao);
                var relativo = CriarPathRelativo(destinoAbsoluto);

                if (ArquivoLocalAtualizado(destinoAbsoluto, item.Hash))
                {
                    AtualizarRegistroImagem(item, relativo);
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destinoAbsoluto) ?? AppContext.BaseDirectory);
                using var response = await httpClient
                    .GetAsync($"api/produtos/{item.ProdutoId}/imagem")
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    continue;

                await using (var origem = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                await using (var destino = new FileStream(destinoAbsoluto, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await origem.CopyToAsync(destino).ConfigureAwait(false);
                }

                if (!string.IsNullOrWhiteSpace(item.Hash) && !ArquivoLocalAtualizado(destinoAbsoluto, item.Hash))
                {
                    File.Delete(destinoAbsoluto);
                    continue;
                }

                RemoverArquivosProdutoExceto(item.ProdutoId, destinoAbsoluto);
                AtualizarRegistroImagem(item, relativo);
                baixadas++;
            }

            return baixadas;
        }

        private void AtualizarRegistroImagem(ProdutoImagemManifestItem item, string relativo)
        {
            var registros = _context.ProdutosImagens.Where(i => i.IdProduto == item.ProdutoId).ToList();
            var registro = registros.FirstOrDefault();
            if (registro == null)
            {
                _context.ProdutosImagens.Add(new ProdutoImagem
                {
                    Id = null,
                    IdProduto = item.ProdutoId,
                    Imagem = relativo
                });
                registro = _context.ProdutosImagens.Local.First(i => i.IdProduto == item.ProdutoId);
            }
            else if (registro.PendenteUpload == "S" || registro.PendenteExclusao == "S")
            {
                return;
            }
            else
            {
                registro.Imagem = relativo;
            }

            AtualizarMetadadosManifest(registro, item);

            foreach (var duplicado in registros.Skip(1))
                _context.ProdutosImagens.Remove(duplicado);

            _context.SaveChanges();
        }

        private void RemoverImagemLocalSeNaoPendente(int produtoId)
        {
            var registros = _context.ProdutosImagens.Where(i => i.IdProduto == produtoId).ToList();
            if (registros.Any(i => i.PendenteUpload == "S" || i.PendenteExclusao == "S"))
                return;

            foreach (var registro in registros)
                _context.ProdutosImagens.Remove(registro);

            RemoverArquivosProdutoExceto(produtoId, null);
            _context.SaveChanges();
        }

        private void AtualizarMetadadosUpload(ProdutoImagem local, ProdutoImagemUploadResponse resposta, string path)
        {
            local.UrlRemota = resposta.Url;
            local.HashSha256 = string.IsNullOrWhiteSpace(resposta.Hash) ? CalcularHashArquivo(path) : resposta.Hash;
            local.TamanhoBytes = resposta.TamanhoBytes > 0 ? resposta.TamanhoBytes : new FileInfo(path).Length;
            local.ContentType = string.IsNullOrWhiteSpace(resposta.ContentType) ? ResolverContentType(path) : resposta.ContentType;
            local.AtualizadoEm = resposta.AtualizadoEm == default ? DateTime.UtcNow : resposta.AtualizadoEm;
            local.SincronizadoEm = DateTime.UtcNow;
            local.PendenteUpload = "N";
            local.PendenteExclusao = "N";
        }

        private void AtualizarMetadadosSincronizados(ProdutoImagem local, ProdutoImagemManifestItem remoto, string path)
        {
            var mudou = false;
            if (!string.Equals(local.UrlRemota, remoto.Url, StringComparison.Ordinal))
            {
                local.UrlRemota = remoto.Url;
                mudou = true;
            }

            var hash = string.IsNullOrWhiteSpace(remoto.Hash) ? CalcularHashArquivo(path) : remoto.Hash;
            if (!string.Equals(local.HashSha256, hash, StringComparison.Ordinal))
            {
                local.HashSha256 = hash;
                mudou = true;
            }

            var tamanho = remoto.TamanhoBytes > 0 ? remoto.TamanhoBytes : new FileInfo(path).Length;
            if (local.TamanhoBytes != tamanho)
            {
                local.TamanhoBytes = tamanho;
                mudou = true;
            }

            var contentType = string.IsNullOrWhiteSpace(remoto.ContentType) ? ResolverContentType(path) : remoto.ContentType;
            if (!string.Equals(local.ContentType, contentType, StringComparison.Ordinal))
            {
                local.ContentType = contentType;
                mudou = true;
            }

            if (remoto.AtualizadoEm != default && local.AtualizadoEm != remoto.AtualizadoEm)
            {
                local.AtualizadoEm = remoto.AtualizadoEm;
                mudou = true;
            }

            if (local.PendenteUpload != "N")
            {
                local.PendenteUpload = "N";
                mudou = true;
            }

            if (local.PendenteExclusao != "N")
            {
                local.PendenteExclusao = "N";
                mudou = true;
            }

            if (mudou || !local.SincronizadoEm.HasValue)
                local.SincronizadoEm = DateTime.UtcNow;
        }

        private void AtualizarMetadadosManifest(ProdutoImagem local, ProdutoImagemManifestItem remoto)
        {
            var mudou = false;
            if (!string.Equals(local.UrlRemota, remoto.Url, StringComparison.Ordinal))
            {
                local.UrlRemota = remoto.Url;
                mudou = true;
            }

            if (!string.Equals(local.HashSha256, remoto.Hash, StringComparison.Ordinal))
            {
                local.HashSha256 = remoto.Hash;
                mudou = true;
            }

            if (local.TamanhoBytes != remoto.TamanhoBytes)
            {
                local.TamanhoBytes = remoto.TamanhoBytes;
                mudou = true;
            }

            if (!string.Equals(local.ContentType, remoto.ContentType, StringComparison.Ordinal))
            {
                local.ContentType = remoto.ContentType;
                mudou = true;
            }

            var atualizadoEm = remoto.AtualizadoEm == default ? DateTime.UtcNow : remoto.AtualizadoEm;
            if (local.AtualizadoEm != atualizadoEm)
            {
                local.AtualizadoEm = atualizadoEm;
                mudou = true;
            }

            if (local.PendenteUpload != "N")
            {
                local.PendenteUpload = "N";
                mudou = true;
            }

            if (local.PendenteExclusao != "N")
            {
                local.PendenteExclusao = "N";
                mudou = true;
            }

            if (mudou || !local.SincronizadoEm.HasValue)
                local.SincronizadoEm = DateTime.UtcNow;
        }

        private string ObterImagemProdutoPath(int produtoId, string extensao)
        {
            return Path.Combine(ObterBaseTenant(), "imagens", "produtos", $"{produtoId}{extensao}");
        }

        private string ObterBaseTenant()
        {
            var databasePath = Path.GetFullPath(_databaseService.CurrentDatabasePath);
            return Path.GetDirectoryName(databasePath) ?? AppContext.BaseDirectory;
        }

        private string CriarPathRelativo(string pathAbsoluto)
        {
            var baseDir = ObterBaseTenant();
            var relativo = Path.GetRelativePath(baseDir, pathAbsoluto);
            return relativo.Replace(Path.DirectorySeparatorChar, '/');
        }

        private static string? ResolverExtensao(ProdutoImagemManifestItem item)
        {
            var contentType = item.ContentType ?? string.Empty;
            if (contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
                return ".jpg";
            if (contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
                return ".png";

            var urlPath = item.Url;
            if (Uri.TryCreate(item.Url, UriKind.Absolute, out var uri))
                urlPath = uri.AbsolutePath;

            var extensao = Path.GetExtension(urlPath);
            return extensao.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                || extensao.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
                || extensao.Equals(".png", StringComparison.OrdinalIgnoreCase)
                    ? extensao.ToLowerInvariant()
                    : null;
        }

        private bool ArquivoLocalAtualizado(string path, string? hashEsperado)
        {
            if (!File.Exists(path))
                return false;

            if (string.IsNullOrWhiteSpace(hashEsperado))
                return true;

            using var stream = File.OpenRead(path);
            var hash = SHA256.HashData(stream);
            return Convert.ToHexString(hash).Equals(hashEsperado, StringComparison.OrdinalIgnoreCase);
        }

        private string? ResolverImagemLocalAbsoluta(string? relativo)
        {
            if (string.IsNullOrWhiteSpace(relativo))
                return null;

            var baseDir = ObterBaseTenant();
            var relativoNormalizado = relativo
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            var path = Path.GetFullPath(Path.Combine(baseDir, relativoNormalizado));

            if (!path.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                return null;

            return File.Exists(path) ? path : null;
        }

        private static string CalcularHashArquivo(string path)
        {
            using var stream = File.OpenRead(path);
            var hash = SHA256.HashData(stream);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static string ResolverContentType(string path)
        {
            var extensao = Path.GetExtension(path);
            if (extensao.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                extensao.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return "image/jpeg";
            }

            if (extensao.Equals(".png", StringComparison.OrdinalIgnoreCase))
                return "image/png";

            return "application/octet-stream";
        }

        private void RemoverArquivosProdutoExceto(int produtoId, string? manterPath)
        {
            var diretorio = Path.Combine(ObterBaseTenant(), "imagens", "produtos");
            if (!Directory.Exists(diretorio))
                return;

            var manter = string.IsNullOrWhiteSpace(manterPath) ? null : Path.GetFullPath(manterPath);
            foreach (var arquivo in Directory.EnumerateFiles(diretorio, $"{produtoId}.*"))
            {
                var arquivoAbsoluto = Path.GetFullPath(arquivo);
                if (manter != null && string.Equals(arquivoAbsoluto, manter, StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Delete(arquivoAbsoluto);
            }
        }

        private void GarantirSchemaProdutoImagem()
        {
            if (_schemaGarantido)
                return;

            var connection = _context.Database.GetDbConnection();
            var deveFechar = connection.State != System.Data.ConnectionState.Open;
            if (deveFechar)
                connection.Open();

            try
            {
                var colunas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA table_info('PRODUTO_IMAGEM')";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                        colunas.Add(reader["name"]?.ToString() ?? string.Empty);
                }

                GarantirColuna(connection, colunas, "UrlRemota", "TEXT");
                GarantirColuna(connection, colunas, "HashSha256", "TEXT");
                GarantirColuna(connection, colunas, "TamanhoBytes", "INTEGER");
                GarantirColuna(connection, colunas, "ContentType", "TEXT");
                GarantirColuna(connection, colunas, "AtualizadoEm", "TEXT");
                GarantirColuna(connection, colunas, "SincronizadoEm", "TEXT");
                GarantirColuna(connection, colunas, "PendenteUpload", "TEXT");
                GarantirColuna(connection, colunas, "PendenteExclusao", "TEXT");

                _schemaGarantido = true;
            }
            finally
            {
                if (deveFechar)
                    connection.Close();
            }
        }

        private static void GarantirColuna(System.Data.Common.DbConnection connection, HashSet<string> colunas, string nome, string tipo)
        {
            if (colunas.Contains(nome))
                return;

            using var command = connection.CreateCommand();
            command.CommandText = $"ALTER TABLE PRODUTO_IMAGEM ADD COLUMN {nome} {tipo}";
            command.ExecuteNonQuery();
            colunas.Add(nome);
        }
    }
}

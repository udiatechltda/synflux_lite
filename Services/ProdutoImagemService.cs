using Microsoft.EntityFrameworkCore;
using PDV.Models.Pdv.Cadastros;
using PDV.Services.Interfaces;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace PDV.Services
{
    public class ProdutoImagemService : IProdutoImagemService
    {
        private static readonly HashSet<string> ExtensoesPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        private readonly PdvContext _context;
        private readonly ILocalDatabaseService _databaseService;
        private bool _schemaGarantido;

        public ProdutoImagemService(
            PdvContext context,
            ILocalDatabaseService databaseService)
        {
            _context = context;
            _databaseService = databaseService;
        }

        public string? ObterImagemLocalPath(int? produtoId)
        {
            GarantirSchemaProdutoImagem();

            if (!produtoId.HasValue)
                return null;

            try
            {
                var imagem = _context.ProdutosImagens
                    .AsNoTracking()
                    .Where(i => i.IdProduto == produtoId)
                    .OrderByDescending(i => i.Id)
                    .Select(i => i.Imagem)
                    .FirstOrDefault();

                return ResolverPathAbsoluto(imagem);
            }
            catch
            {
                return null;
            }
        }

        public IReadOnlyDictionary<int, string> ObterImagensLocalPath(IEnumerable<int?> produtoIds)
        {
            GarantirSchemaProdutoImagem();

            var ids = produtoIds
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
                return new Dictionary<int, string>();

            try
            {
                var registros = _context.ProdutosImagens
                    .AsNoTracking()
                    .Where(i => i.IdProduto.HasValue && ids.Contains(i.IdProduto.Value))
                    .OrderByDescending(i => i.Id)
                    .ToList();

                return registros
                    .GroupBy(i => i.IdProduto!.Value)
                    .Select(g => new { ProdutoId = g.Key, Path = ResolverPathAbsoluto(g.First().Imagem) })
                    .Where(i => !string.IsNullOrWhiteSpace(i.Path))
                    .ToDictionary(i => i.ProdutoId, i => i.Path!);
            }
            catch
            {
                return new Dictionary<int, string>();
            }
        }

        public void SalvarImagemProduto(int? produtoId, string origemPath)
        {
            GarantirSchemaProdutoImagem();

            if (!produtoId.HasValue)
                throw new InvalidOperationException("Produto sem ID para vincular imagem.");

            if (string.IsNullOrWhiteSpace(origemPath) || !File.Exists(origemPath))
                throw new InvalidOperationException("Arquivo de imagem nao encontrado.");

            var extensao = Path.GetExtension(origemPath);
            if (!ExtensoesPermitidas.Contains(extensao))
                throw new InvalidOperationException("Formato de imagem invalido. Use JPG ou PNG.");

            var diretorio = ObterDiretorioProdutos();
            Directory.CreateDirectory(diretorio);

            var destino = Path.Combine(diretorio, $"{produtoId.Value}{extensao.ToLowerInvariant()}");
            var origemAbsoluta = Path.GetFullPath(origemPath);
            var destinoAbsoluto = Path.GetFullPath(destino);

            if (!string.Equals(origemAbsoluta, destinoAbsoluto, StringComparison.OrdinalIgnoreCase))
            {
                RemoverArquivosProduto(produtoId.Value);
                File.Copy(origemAbsoluta, destinoAbsoluto, overwrite: true);
            }

            var relativo = CriarPathRelativo(destinoAbsoluto);
            var hash = CalcularHashArquivo(destinoAbsoluto);
            var info = new FileInfo(destinoAbsoluto);
            var atualizadoEm = DateTime.UtcNow;
            var registro = _context.ProdutosImagens.FirstOrDefault(i => i.IdProduto == produtoId);
            if (registro == null)
            {
                _context.ProdutosImagens.Add(new ProdutoImagem
                {
                    Id = null,
                    IdProduto = produtoId,
                    Imagem = relativo,
                    HashSha256 = hash,
                    TamanhoBytes = info.Length,
                    ContentType = ResolverContentType(destinoAbsoluto),
                    AtualizadoEm = atualizadoEm,
                    SincronizadoEm = null,
                    UrlRemota = null,
                    PendenteUpload = "S",
                    PendenteExclusao = "N"
                });
            }
            else
            {
                registro.Imagem = relativo;
                registro.HashSha256 = hash;
                registro.TamanhoBytes = info.Length;
                registro.ContentType = ResolverContentType(destinoAbsoluto);
                registro.AtualizadoEm = atualizadoEm;
                registro.SincronizadoEm = null;
                registro.UrlRemota = null;
                registro.PendenteUpload = "S";
                registro.PendenteExclusao = "N";
                _context.ProdutosImagens.Update(registro);
            }

            _context.SaveChanges();
        }

        public void RemoverImagemProduto(int? produtoId)
        {
            GarantirSchemaProdutoImagem();

            if (!produtoId.HasValue)
                return;

            var registros = _context.ProdutosImagens
                .Where(i => i.IdProduto == produtoId)
                .ToList();

            foreach (var registro in registros)
            {
                if (TemImagemRemota(registro))
                {
                    registro.Imagem = null;
                    registro.PendenteUpload = "N";
                    registro.PendenteExclusao = "S";
                    registro.AtualizadoEm = DateTime.UtcNow;
                    _context.ProdutosImagens.Update(registro);
                    continue;
                }

                _context.ProdutosImagens.Remove(registro);
            }

            RemoverArquivosProduto(produtoId.Value);
            _context.SaveChanges();
        }

        private string? ResolverPathAbsoluto(string? pathRelativo)
        {
            if (string.IsNullOrWhiteSpace(pathRelativo))
                return null;

            var baseDir = ObterBaseTenant();
            var relativoNormalizado = pathRelativo
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            var path = Path.GetFullPath(Path.Combine(baseDir, relativoNormalizado));

            if (!path.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                return null;

            return File.Exists(path) ? path : null;
        }

        private string ObterDiretorioProdutos()
        {
            return Path.Combine(ObterBaseTenant(), "imagens", "produtos");
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

        private void RemoverArquivosProduto(int produtoId)
        {
            var diretorio = ObterDiretorioProdutos();
            if (!Directory.Exists(diretorio))
                return;

            foreach (var arquivo in Directory.EnumerateFiles(diretorio, $"{produtoId}.*"))
                File.Delete(arquivo);
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

        private static string CalcularHashArquivo(string path)
        {
            using var stream = File.OpenRead(path);
            var hash = SHA256.HashData(stream);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static bool TemImagemRemota(ProdutoImagem registro)
        {
            return !string.IsNullOrWhiteSpace(registro.UrlRemota) ||
                   registro.SincronizadoEm.HasValue;
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

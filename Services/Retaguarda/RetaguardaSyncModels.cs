using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PDV.Services.Retaguarda
{
    public sealed class PdvSnapshotRequest
    {
        [JsonPropertyName("dispositivoId")]
        public string DispositivoId { get; set; } = string.Empty;

        [JsonPropertyName("tabelas")]
        public List<PdvSnapshotTable> Tabelas { get; set; } = new();
    }

    public sealed class PdvSnapshotTable
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("registros")]
        public List<PdvSnapshotRecord> Registros { get; set; } = new();
    }

    public sealed class PdvSnapshotRecord
    {
        [JsonPropertyName("idLocal")]
        public string IdLocal { get; set; } = string.Empty;

        [JsonPropertyName("dadosJson")]
        public string DadosJson { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;
    }

    public sealed class PdvSnapshotResponse
    {
        [JsonPropertyName("bancoOperacional")]
        public string BancoOperacional { get; set; } = string.Empty;

        [JsonPropertyName("totalTabelas")]
        public int TotalTabelas { get; set; }

        [JsonPropertyName("totalRegistros")]
        public int TotalRegistros { get; set; }
    }

    public sealed class ProdutoImagemManifestResponse
    {
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = string.Empty;

        [JsonPropertyName("imagens")]
        public List<ProdutoImagemManifestItem> Imagens { get; set; } = new();
    }

    public sealed class ProdutoImagemManifestItem
    {
        [JsonPropertyName("produtoId")]
        public int ProdutoId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("caminhoRelativo")]
        public string CaminhoRelativo { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("tamanhoBytes")]
        public long TamanhoBytes { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [JsonPropertyName("atualizadoEm")]
        public DateTime AtualizadoEm { get; set; }

        [JsonPropertyName("excluido")]
        public bool Excluido { get; set; }
    }

    public sealed class ProdutoImagemUploadResponse
    {
        [JsonPropertyName("produtoId")]
        public int ProdutoId { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("caminhoRelativo")]
        public string CaminhoRelativo { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("tamanhoBytes")]
        public long TamanhoBytes { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [JsonPropertyName("atualizadoEm")]
        public DateTime AtualizadoEm { get; set; }
    }
}

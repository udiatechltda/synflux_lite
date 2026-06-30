using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO_IMAGEM")]
    public class ProdutoImagem
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProduto { get; set; }
        public string? Imagem { get; set; }
        public string? UrlRemota { get; set; }
        public string? HashSha256 { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? ContentType { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public DateTime? SincronizadoEm { get; set; }
        public string? PendenteUpload { get; set; }
        public string? PendenteExclusao { get; set; }
    }
}

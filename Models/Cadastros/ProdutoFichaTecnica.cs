using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO_FICHA_TECNICA")]
    public class ProdutoFichaTecnica
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProduto { get; set; }
        public string? Descricao { get; set; }
        public int? IdProdutoFilho { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorCusto { get; set; }
        public double? PercentualCusto { get; set; }
    }
}

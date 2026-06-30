using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO_SUBGRUPO")]
    public class ProdutoSubgrupo
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProdutoGrupo { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
    }
}

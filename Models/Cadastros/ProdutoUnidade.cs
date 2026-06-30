using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO_UNIDADE")]
    public class ProdutoUnidade
    {
        [Key]
        public int? Id { get; set; }
        public string? Sigla { get; set; }
        public string? Descricao { get; set; }
        public string? PodeFracionar { get; set; }
    }
}

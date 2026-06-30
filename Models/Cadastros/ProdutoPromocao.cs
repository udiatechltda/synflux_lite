using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO_PROMOCAO")]
    public class ProdutoPromocao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProduto { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public double? QuantidadeEmPromocao { get; set; }
        public double? QuantidadeMaximaCliente { get; set; }
        public double? Valor { get; set; }
    }
}

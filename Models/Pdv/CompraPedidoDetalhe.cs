using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMPRA_PEDIDO_DETALHE")]
    public class CompraPedidoDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCompraPedidoCabecalho { get; set; }
        public int? IdProduto { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorUnitario { get; set; }
        public double? ValorSubtotal { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorTotal { get; set; }
        public string? Cst { get; set; }
        public string? Csosn { get; set; }
        public int? Cfop { get; set; }
    }
}

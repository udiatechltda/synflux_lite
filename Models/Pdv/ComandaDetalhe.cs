using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMANDA_DETALHE")]
    public class ComandaDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdComanda { get; set; }
        public int? IdProduto { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorUnitario { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorTotalComplemento { get; set; }
        public string? Observacao { get; set; }
        public string? GerouPedidoCozinha { get; set; }
    }
}

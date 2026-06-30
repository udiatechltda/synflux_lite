using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMANDA_DETALHE_COMPLEMENTO")]
    public class ComandaDetalheComplemento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdComandaDetalhe { get; set; }
        public int? IdProduto { get; set; }
        public string? NomeProduto { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorUnitario { get; set; }
        public double? ValorTotal { get; set; }
    }
}

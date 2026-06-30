using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("TAXA_ENTREGA")]
    public class TaxaEntrega
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public double? Valor { get; set; }
        public int? EstimativaMinutos { get; set; }
    }
}

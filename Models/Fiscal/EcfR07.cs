using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_R07")]
    public class EcfR07
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEcfR06 { get; set; }
        public int? Ccf { get; set; }
        public string? MeioPagamento { get; set; }
        public double? ValorPagamento { get; set; }
        public string? Estorno { get; set; }
        public double? ValorEstorno { get; set; }
        public string? HashRegistro { get; set; }
    }
}

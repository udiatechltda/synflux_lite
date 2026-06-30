using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_R03")]
    public class EcfR03
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEcfR02 { get; set; }
        public string? SerieEcf { get; set; }
        public string? TotalizadorParcial { get; set; }
        public double? ValorAcumulado { get; set; }
        public int? Crz { get; set; }
        public string? HashRegistro { get; set; }
    }
}

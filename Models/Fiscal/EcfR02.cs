using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_R02")]
    public class EcfR02
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvOperador { get; set; }
        public int? IdEcfImpressora { get; set; }
        public int? IdEcfCaixa { get; set; }
        public string? SerieEcf { get; set; }
        public int? Crz { get; set; }
        public int? Coo { get; set; }
        public int? Cro { get; set; }
        public DateTime? DataMovimento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string? HoraEmissao { get; set; }
        public double? VendaBruta { get; set; }
        public double? GrandeTotal { get; set; }
        public string? HashRegistro { get; set; }
    }
}

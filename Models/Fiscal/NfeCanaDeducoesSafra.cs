using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CANA_DEDUCOES_SAFRA")]
    public class NfeCanaDeducoesSafra
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCana { get; set; }
        public string? Decricao { get; set; }
        public double? ValorDeducao { get; set; }
        public double? ValorFornecimento { get; set; }
        public double? ValorTotalDeducao { get; set; }
        public double? ValorLiquidoFornecimento { get; set; }
    }
}

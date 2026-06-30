using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_ICMS_UFDEST")]
    public class NfeDetalheImpostoIcmsUfdest
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public double? ValorBcIcmsUfDestino { get; set; }
        public double? ValorBcFcpUfDestino { get; set; }
        public double? PercentualFcpUfDestino { get; set; }
        public double? AliquotaInternaUfDestino { get; set; }
        public double? AliquotaInteresdatualUfEnvolvidas { get; set; }
        public double? PercentualProvisorioPartilhaIcms { get; set; }
        public double? ValorIcmsFcpUfDestino { get; set; }
        public double? ValorInterestadualUfDestino { get; set; }
        public double? ValorInterestadualUfRemetente { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("TRIBUT_ICMS_CUSTOM_DET")]
    public class TributIcmsCustomDet
    {
        [Key]
        public int? Id { get; set; }
        public int? IdTributIcmsCustomCab { get; set; }
        public string? UfDestino { get; set; }
        public int? Cfop { get; set; }
        public string? Csosn { get; set; }
        public string? Cst { get; set; }
        public string? ModalidadeBc { get; set; }
        public double? Aliquota { get; set; }
        public double? ValorPauta { get; set; }
        public double? ValorPrecoMaximo { get; set; }
        public double? Mva { get; set; }
        public double? PorcentoBc { get; set; }
        public string? ModalidadeBcSt { get; set; }
        public double? AliquotaInternaSt { get; set; }
        public double? AliquotaInterestadualSt { get; set; }
        public double? PorcentoBcSt { get; set; }
        public double? AliquotaIcmsSt { get; set; }
        public double? ValorPautaSt { get; set; }
        public double? ValorPrecoMaximoSt { get; set; }
    }
}

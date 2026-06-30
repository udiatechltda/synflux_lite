using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("TRIBUT_IPI")]
    public class TributIpi
    {
        [Key]
        public int? Id { get; set; }
        public int? IdTributConfiguraOfGt { get; set; }
        public string? CstIpi { get; set; }
        public string? ModalidadeBaseCalculo { get; set; }
        public double? PorcentoBaseCalculo { get; set; }
        public double? AliquotaPorcento { get; set; }
        public double? AliquotaUnidade { get; set; }
        public double? ValorPrecoMaximo { get; set; }
        public double? ValorPautaFiscal { get; set; }
    }
}

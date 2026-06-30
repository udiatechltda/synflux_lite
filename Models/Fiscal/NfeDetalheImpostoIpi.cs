using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_IPI")]
    public class NfeDetalheImpostoIpi
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? CnpjProdutor { get; set; }
        public string? CodigoSeloIpi { get; set; }
        public int? QuantidadeSeloIpi { get; set; }
        public string? EnquadramentoLegalIpi { get; set; }
        public string? CstIpi { get; set; }
        public double? ValorBaseCalculoIpi { get; set; }
        public double? QuantidadeUnidadeTributavel { get; set; }
        public double? ValorUnidadeTributavel { get; set; }
        public double? AliquotaIpi { get; set; }
        public double? ValorIpi { get; set; }
    }
}

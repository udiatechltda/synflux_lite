using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_COFINS_ST")]
    public class NfeDetalheImpostoCofinsSt
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public double? BaseCalculoCofinsSt { get; set; }
        public double? AliquotaCofinsStPercentual { get; set; }
        public double? QuantidadeVendidaCofinsSt { get; set; }
        public double? AliquotaCofinsStReais { get; set; }
        public double? ValorCofinsSt { get; set; }
    }
}

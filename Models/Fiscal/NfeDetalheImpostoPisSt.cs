using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_PIS_ST")]
    public class NfeDetalheImpostoPisSt
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public double? ValorBaseCalculoPisSt { get; set; }
        public double? AliquotaPisStPercentual { get; set; }
        public double? QuantidadeVendidaPisSt { get; set; }
        public double? AliquotaPisStReais { get; set; }
        public double? ValorPisSt { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_PIS")]
    public class NfeDetalheImpostoPis
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? CstPis { get; set; }
        public double? ValorBaseCalculoPis { get; set; }
        public double? AliquotaPisPercentual { get; set; }
        public double? ValorPis { get; set; }
        public double? QuantidadeVendida { get; set; }
        public double? AliquotaPisReais { get; set; }
    }
}

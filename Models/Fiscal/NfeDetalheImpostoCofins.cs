using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_COFINS")]
    public class NfeDetalheImpostoCofins
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? CstCofins { get; set; }
        public double? BaseCalculoCofins { get; set; }
        public double? AliquotaCofinsPercentual { get; set; }
        public double? QuantidadeVendida { get; set; }
        public double? AliquotaCofinsReais { get; set; }
        public double? ValorCofins { get; set; }
    }
}

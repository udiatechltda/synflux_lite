using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_II")]
    public class NfeDetalheImpostoIi
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public double? ValorBcIi { get; set; }
        public double? ValorDespesasAduaneiras { get; set; }
        public double? ValorImpostoImportacao { get; set; }
        public double? ValorIof { get; set; }
    }
}

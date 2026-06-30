using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CANA_FORNECIMENTO_DIARIO")]
    public class NfeCanaFornecimentoDiario
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCana { get; set; }
        public string? Dia { get; set; }
        public double? Quantidade { get; set; }
        public double? QuantidadeTotalMes { get; set; }
        public double? QuantidadeTotalAnterior { get; set; }
        public double? QuantidadeTotalGeral { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_FATURA")]
    public class NfeFatura
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? Numero { get; set; }
        public double? ValorOriginal { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorLiquido { get; set; }
    }
}

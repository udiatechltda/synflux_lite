using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DUPLICATA")]
    public class NfeDuplicata
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeFatura { get; set; }
        public string? Numero { get; set; }
        public DateTime? DataVencimento { get; set; }
        public double? Valor { get; set; }
    }
}

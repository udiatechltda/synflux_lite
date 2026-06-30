using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_LOG_TOTAIS")]
    public class EcfLogTotais
    {
        [Key]
        public int? Id { get; set; }
        public int? TipoPagamento { get; set; }
        public int? Produto { get; set; }
        public int? R01 { get; set; }
        public int? R02 { get; set; }
        public int? R03 { get; set; }
        public int? R04 { get; set; }
        public int? R05 { get; set; }
        public int? R06 { get; set; }
        public int? R07 { get; set; }
    }
}

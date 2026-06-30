using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_FECHAMENTO")]
    public class PdvFechamento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvMovimento { get; set; }
        public int? IdPdvTipoPagamento { get; set; }
        public double? Valor { get; set; }
    }
}

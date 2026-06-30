using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_SINTEGRA_60A")]
    public class EcfSintegra60A
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEcfSintegra60M { get; set; }
        public string? SituacaoTributaria { get; set; }
        public double? Valor { get; set; }
    }
}

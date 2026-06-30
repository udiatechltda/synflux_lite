using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("CFOP")]
    public class Cfop
    {
        [Key]
        public int? Id { get; set; }
        public int? Codigo { get; set; }
        public string? Descricao { get; set; }
        public string? Aplicacao { get; set; }
    }
}

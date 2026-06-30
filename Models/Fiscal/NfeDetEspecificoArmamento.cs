using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DET_ESPECIFICO_ARMAMENTO")]
    public class NfeDetEspecificoArmamento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? TipoArma { get; set; }
        public string? NumeroSerieArma { get; set; }
        public string? NumeroSerieCano { get; set; }
        public string? Descricao { get; set; }
    }
}

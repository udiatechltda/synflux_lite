using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DET_ESPECIFICO_MEDICAMENTO")]
    public class NfeDetEspecificoMedicamento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? CodigoAnvisa { get; set; }
        public string? MotivoIsencao { get; set; }
        public double? PrecoMaximoConsumidor { get; set; }
    }
}

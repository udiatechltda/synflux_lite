using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_SANGRIA")]
    public class PdvSangria
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvMovimento { get; set; }
        public DateTime? DataSangria { get; set; }
        public string? HoraSangria { get; set; }
        public double? Valor { get; set; }
        public string? Observacao { get; set; }
    }
}

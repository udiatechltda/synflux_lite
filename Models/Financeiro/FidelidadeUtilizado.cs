using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("FIDELIDADE_UTILIZADO")]
    public class FidelidadeUtilizado
    {
        [Key]
        public int? Id { get; set; }
        public DateTime? DataUtilizacao { get; set; }
        public string? HoraUtilizacao { get; set; }
        public double? ValorUtilizado { get; set; }
    }
}

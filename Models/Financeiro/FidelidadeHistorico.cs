using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("FIDELIDADE_HISTORICO")]
    public class FidelidadeHistorico
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdFidelidadeUtilizado { get; set; }
        public DateTime? DataConsumo { get; set; }
        public string? HoraConsumo { get; set; }
        public double? ValorConsumo { get; set; }
    }
}

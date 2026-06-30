using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("ENTREGADOR_ROTA")]
    public class EntregadorRota
    {
        [Key]
        public int? Id { get; set; }
        public int? IdColaborador { get; set; }
        public DateTime? DataRota { get; set; }
        public string? HoraSaida { get; set; }
        public int? EstimativaMinutos { get; set; }
        public string? HoraPrevistoRetorno { get; set; }
    }
}

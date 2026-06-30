using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMANDA_PEDIDO")]
    public class ComandaPedido
    {
        [Key]
        public int? Id { get; set; }
        public int? IdComanda { get; set; }
        public int? IdCozinha { get; set; }
        public DateTime? EntrouNaFila { get; set; }
        public DateTime? SaiuDaFila { get; set; }
        public int? EstimativaMinutos { get; set; }
        public int? Posicao { get; set; }
        public string? Prioridade { get; set; }
        public DateTime? InicioPreparo { get; set; }
        public DateTime? FimPreparo { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("MESA")]
    public class Mesa
    {
        [Key]
        public int? Id { get; set; }
        public string? Numero { get; set; }
        public int? QuantidadeCadeiras { get; set; }
        public int? QuantidadeCadeirasCrianca { get; set; }
        public string? Disponivel { get; set; }
        public string? Observacao { get; set; }
    }
}

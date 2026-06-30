using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("RESERVA_MESA")]
    public class ReservaMesa
    {
        [Key]
        public int? Id { get; set; }
        public int? IdMesa { get; set; }
        public int? IdReserva { get; set; }
    }
}

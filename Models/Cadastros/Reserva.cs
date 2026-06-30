using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("RESERVA")]
    public class Reserva
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCliente { get; set; }
        public string? NomeContato { get; set; }
        public string? TelefoneContato { get; set; }
        public DateTime? DataReserva { get; set; }
        public string? HoraReserva { get; set; }
        public int? QuantidadePessoas { get; set; }
        public string? Situacao { get; set; }
    }
}

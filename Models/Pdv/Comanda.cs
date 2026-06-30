using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMANDA")]
    public class Comanda
    {
        [Key]
        public int? Id { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdMesa { get; set; }
        public int? IdCliente { get; set; }
        public int? IdEmpresaDeliveryPedido { get; set; }
        public int? Numero { get; set; }
        public DateTime? DataChegada { get; set; }
        public string? HoraChegada { get; set; }
        public DateTime? DataSaida { get; set; }
        public string? HoraSaida { get; set; }
        public double? ValorSubtotal { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorTotal { get; set; }
        public string? Tipo { get; set; }
        public int? QuantidadePessoas { get; set; }
        public double? ValorPorPessoa { get; set; }
        public string? Situacao { get; set; }
        public int? CodigoCompartilhado { get; set; }
    }
}

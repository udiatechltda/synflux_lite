using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("DELIVERY_ACERTO")]
    public class DeliveryAcerto
    {
        [Key]
        public int? Id { get; set; }
        public DateTime? DataAcerto { get; set; }
        public string? HoraAcerto { get; set; }
        public double? ValorRecebido { get; set; }
        public double? ValorPagoEntregador { get; set; }
        public string? Observacao { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("DELIVERY_ACERTO_COMANDA")]
    public class DeliveryAcertoComanda
    {
        [Key]
        public int? Id { get; set; }
        public int? IdDeliveryAcerto { get; set; }
        public int? IdDelivery { get; set; }
    }
}

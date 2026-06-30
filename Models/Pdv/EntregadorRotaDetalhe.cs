using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("ENTREGADOR_ROTA_DETALHE")]
    public class EntregadorRotaDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEntregadorRota { get; set; }
        public int? IdDelivery { get; set; }
        public int? PosicaoNaFila { get; set; }
        public int? Latitude { get; set; }
        public int? Longitude { get; set; }
    }
}

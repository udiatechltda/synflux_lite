using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("EMPRESA_DELIVERY_PEDIDO")]
    public class EmpresaDeliveryPedido
    {
        [Key]
        public int? Id { get; set; }
        public string? CodigoPedidoEmpresa { get; set; }
        public string? ConteudoJson { get; set; }
        public string? Observacao { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public string? HoraSolicitacao { get; set; }
    }
}

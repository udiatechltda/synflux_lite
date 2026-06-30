using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("CLIENTE_FIADO")]
    public class ClienteFiado
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdPdvVendaCabecalho { get; set; }
        public double? ValorPendente { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataLancamento { get; set; }
    }
}

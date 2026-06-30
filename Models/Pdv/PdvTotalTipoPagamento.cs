using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_TOTAL_TIPO_PAGAMENTO")]
    public class PdvTotalTipoPagamento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvVendaCabecalho { get; set; }
        public int? IdPdvTipoPagamento { get; set; }
        public DateTime? DataVenda { get; set; }
        public string? HoraVenda { get; set; }
        public string? SerieEcf { get; set; }
        public int? Coo { get; set; }
        public int? Ccf { get; set; }
        public int? Gnf { get; set; }
        public double? Valor { get; set; }
        public string? Nsu { get; set; }
        public string? Estorno { get; set; }
        public string? Rede { get; set; }
        public string? CartaoDc { get; set; }
        public string? HashRegistro { get; set; }
    }
}

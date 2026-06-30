using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_INFORMACAO_PAGAMENTO")]
    public class NfeInformacaoPagamento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? IndicadorPagamento { get; set; }
        public string? MeioPagamento { get; set; }
        public double? Valor { get; set; }
        public string? TipoIntegracao { get; set; }
        public string? CnpjOperadoraCartao { get; set; }
        public string? Bandeira { get; set; }
        public string? NumeroAutorizacao { get; set; }
        public double? Troco { get; set; }
    }
}

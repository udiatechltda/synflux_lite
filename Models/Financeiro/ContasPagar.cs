using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("CONTAS_PAGAR")]
    public class ContasPagar
    {
        [Key]
        public int? Id { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCompraPedidoCabecalho { get; set; }
        public DateTime? DataLancamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public double? ValorAPagar { get; set; }
        public double? TaxaJuro { get; set; }
        public double? TaxaMulta { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorJuro { get; set; }
        public double? ValorMulta { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorPago { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Historico { get; set; }
        public string? StatusPagamento { get; set; }
    }
}

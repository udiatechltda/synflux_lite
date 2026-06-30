using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("CONTAS_RECEBER")]
    public class ContasReceber
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdPdvVendaCabecalho { get; set; }
        public DateTime? DataLancamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public double? ValorAReceber { get; set; }
        public double? TaxaJuro { get; set; }
        public double? TaxaMulta { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorJuro { get; set; }
        public double? ValorMulta { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorRecebido { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Historico { get; set; }
        public string? StatusRecebimento { get; set; }
    }
}

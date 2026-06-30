using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMPRA_PEDIDO_CABECALHO")]
    public class CompraPedidoCabecalho
    {
        [Key]
        public int? Id { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdFornecedor { get; set; }
        public DateTime? DataPedido { get; set; }
        public DateTime? DataPrevisaoEntrega { get; set; }
        public DateTime? DataPrevisaoPagamento { get; set; }
        public string? LocalEntrega { get; set; }
        public string? LocalCobranca { get; set; }
        public string? Contato { get; set; }
        public double? ValorSubtotal { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorTotal { get; set; }
        public string? FormaPagamento { get; set; }
        public string? GeraFinanceiro { get; set; }
        public int? QuantidadeParcelas { get; set; }
        public DateTime? DiaPrimeiroVencimento { get; set; }
        public int? IntervaloEntreParcelas { get; set; }
        public string? DiaFixoParcela { get; set; }
        public DateTime? DataRecebimentoItens { get; set; }
        public string? HoraRecebimentoItens { get; set; }
        public string? AtualizouEstoque { get; set; }
        public string? NumeroDocumentoEntrada { get; set; }
    }
}

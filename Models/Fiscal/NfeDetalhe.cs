using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE")]
    public class NfeDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public int? NumeroItem { get; set; }
        public string? CodigoProduto { get; set; }
        public string? Gtin { get; set; }
        public string? NomeProduto { get; set; }
        public string? Ncm { get; set; }
        public string? Nve { get; set; }
        public string? Cest { get; set; }
        public string? IndicadorEscalaRelevante { get; set; }
        public string? CnpjFabricante { get; set; }
        public string? CodigoBeneficioFiscal { get; set; }
        public int? ExTipi { get; set; }
        public int? Cfop { get; set; }
        public string? UnidadeComercial { get; set; }
        public double? QuantidadeComercial { get; set; }
        public string? NumeroPedidoCompra { get; set; }
        public int? ItemPedidoCompra { get; set; }
        public string? NumeroFci { get; set; }
        public string? NumeroRecopi { get; set; }
        public double? ValorUnitarioComercial { get; set; }
        public double? ValorBrutoProduto { get; set; }
        public string? GtinUnidadeTributavel { get; set; }
        public string? UnidadeTributavel { get; set; }
        public double? QuantidadeTributavel { get; set; }
        public double? ValorUnitarioTributavel { get; set; }
        public double? ValorFrete { get; set; }
        public double? ValorSeguro { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorOutrasDespesas { get; set; }
        public string? EntraTotal { get; set; }
        public double? ValorTotalTributos { get; set; }
        public double? PercentualDevolvido { get; set; }
        public double? ValorIpiDevolvido { get; set; }
        public string? InformacoesAdicionais { get; set; }
        public double? ValorSubtotal { get; set; }
        public double? ValorTotal { get; set; }
    }
}

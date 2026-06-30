using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_VENDA_CABECALHO")]
    public class PdvVendaCabecalho
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCliente { get; set; }
        public int? IdColaborador { get; set; }
        public int? IdPdvMovimento { get; set; }
        public int? IdEcfDav { get; set; }
        public int? IdEcfPreVendaCabecalho { get; set; }
        public string? SerieEcf { get; set; }
        public int? Cfop { get; set; }
        public int? Coo { get; set; }
        public int? Ccf { get; set; }
        public DateTime? DataVenda { get; set; }
        public string? HoraVenda { get; set; }
        public double? ValorVenda { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorDesconto { get; set; }
        public double? TaxaAcrescimo { get; set; }
        public double? ValorAcrescimo { get; set; }
        public double? ValorFinal { get; set; }
        public double? ValorRecebido { get; set; }
        public double? ValorTroco { get; set; }
        public double? ValorCancelado { get; set; }
        public double? ValorTotalProdutos { get; set; }
        public double? ValorTotalDocumento { get; set; }
        public double? ValorBaseIcms { get; set; }
        public double? ValorIcms { get; set; }
        public double? ValorIcmsOutras { get; set; }
        public double? ValorIssqn { get; set; }
        public double? ValorPis { get; set; }
        public double? ValorCofins { get; set; }
        public double? ValorAcrescimoItens { get; set; }
        public double? ValorDescontoItens { get; set; }
        public string? StatusVenda { get; set; }
        public string? NomeCliente { get; set; }
        public string? CpfCnpjCliente { get; set; }
        public string? CupomCancelado { get; set; }
        public string? HashRegistro { get; set; }
        public string? TipoOperacao { get; set; }
    }
}

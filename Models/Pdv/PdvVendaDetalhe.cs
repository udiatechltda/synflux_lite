using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_VENDA_DETALHE")]
    public class PdvVendaDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProduto { get; set; }
        public int? IdPdvVendaCabecalho { get; set; }
        public int? Cfop { get; set; }
        public string? Gtin { get; set; }
        public int? Ccf { get; set; }
        public int? Coo { get; set; }
        public string? SerieEcf { get; set; }
        public int? Item { get; set; }
        public double? Quantidade { get; set; }
        public double? ValorUnitario { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorTotalItem { get; set; }
        public double? ValorBaseIcms { get; set; }
        public double? TaxaIcms { get; set; }
        public double? ValorIcms { get; set; }
        public double? TaxaDesconto { get; set; }
        public double? ValorDesconto { get; set; }
        public double? TaxaIssqn { get; set; }
        public double? ValorIssqn { get; set; }
        public double? TaxaPis { get; set; }
        public double? ValorPis { get; set; }
        public double? TaxaCofins { get; set; }
        public double? ValorCofins { get; set; }
        public double? TaxaAcrescimo { get; set; }
        public double? ValorAcrescimo { get; set; }
        public string? TotalizadorParcial { get; set; }
        public string? Cst { get; set; }
        public string? Cancelado { get; set; }
        public string? MovimentaEstoque { get; set; }
        public string? EcfIcmsSt { get; set; }
        public double? ValorImpostoFederal { get; set; }
        public double? ValorImpostoEstadual { get; set; }
        public double? ValorImpostoMunicipal { get; set; }
        public string? HashRegistro { get; set; }
    }
}

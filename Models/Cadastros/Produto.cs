using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("PRODUTO")]
    public class Produto
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProdutoUnidade { get; set; }
        public int? IdTributGrupoTributario { get; set; }
        public int? IdProdutoTipo { get; set; }
        public int? IdProdutoSubgrupo { get; set; }
        public string? Gtin { get; set; }
        public string? CodigoInterno { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? DescricaoPdv { get; set; }
        public double? ValorCompra { get; set; }
        public double? ValorVenda { get; set; }
        public double? QuantidadeEstoque { get; set; }
        public double? EstoqueMinimo { get; set; }
        public double? EstoqueMaximo { get; set; }
        public string? CodigoNcm { get; set; }
        public string? Iat { get; set; }
        public string? Ippt { get; set; }
        public string? TipoItemSped { get; set; }
        public double? TaxaIpi { get; set; }
        public double? TaxaIssqn { get; set; }
        public double? TaxaPis { get; set; }
        public double? TaxaCofins { get; set; }
        public double? TaxaIcms { get; set; }
        public string? Cst { get; set; }
        public string? Csosn { get; set; }
        public string? TotalizadorParcial { get; set; }
        public string? EcfIcmsSt { get; set; }
        public int? CodigoBalanca { get; set; }
        public string? PafPSt { get; set; }
        public string? HashRegistro { get; set; }
        public double? ValorCusto { get; set; }
        public string? Situacao { get; set; }
        public string? CodigoCest { get; set; }
        public string? Localizacao { get; set; }
    }
}

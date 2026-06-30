using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_ISSQN")]
    public class NfeDetalheImpostoIssqn
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public double? BaseCalculoIssqn { get; set; }
        public double? AliquotaIssqn { get; set; }
        public double? ValorIssqn { get; set; }
        public int? MunicipioIssqn { get; set; }
        public int? ItemListaServicos { get; set; }
        public double? ValorDeducao { get; set; }
        public double? ValorOutrasRetencoes { get; set; }
        public double? ValorDescontoIncondicionado { get; set; }
        public double? ValorDescontoCondicionado { get; set; }
        public double? ValorRetencaoIss { get; set; }
        public string? IndicadorExigibilidadeIss { get; set; }
        public string? CodigoServico { get; set; }
        public int? MunicipioIncidencia { get; set; }
        public int? PaisSevicoPrestado { get; set; }
        public string? NumeroProcesso { get; set; }
        public string? IndicadorIncentivoFiscal { get; set; }
    }
}

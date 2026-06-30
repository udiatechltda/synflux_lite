using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DETALHE_IMPOSTO_ICMS")]
    public class NfeDetalheImpostoIcms
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? OrigemMercadoria { get; set; }
        public string? CstIcms { get; set; }
        public string? Csosn { get; set; }
        public string? ModalidadeBcIcms { get; set; }
        public double? PercentualReducaoBcIcms { get; set; }
        public double? ValorBcIcms { get; set; }
        public double? AliquotaIcms { get; set; }
        public double? ValorIcmsOperacao { get; set; }
        public double? PercentualDiferimento { get; set; }
        public double? ValorIcmsDiferido { get; set; }
        public double? ValorIcms { get; set; }
        public double? BaseCalculoFcp { get; set; }
        public double? PercentualFcp { get; set; }
        public double? ValorFcp { get; set; }
        public string? ModalidadeBcIcmsSt { get; set; }
        public double? PercentualMvaIcmsSt { get; set; }
        public double? PercentualReducaoBcIcmsSt { get; set; }
        public double? ValorBaseCalculoIcmsSt { get; set; }
        public double? AliquotaIcmsSt { get; set; }
        public double? ValorIcmsSt { get; set; }
        public double? BaseCalculoFcpSt { get; set; }
        public double? PercentualFcpSt { get; set; }
        public double? ValorFcpSt { get; set; }
        public string? UfSt { get; set; }
        public double? PercentualBcOperacaoPropria { get; set; }
        public double? ValorBcIcmsStRetido { get; set; }
        public double? AliquotaSuportadaConsumidor { get; set; }
        public double? ValorIcmsSubstituto { get; set; }
        public double? ValorIcmsStRetido { get; set; }
        public double? BaseCalculoFcpStRetido { get; set; }
        public double? PercentualFcpStRetido { get; set; }
        public double? ValorFcpStRetido { get; set; }
        public string? MotivoDesoneracaoIcms { get; set; }
        public double? ValorIcmsDesonerado { get; set; }
        public double? AliquotaCreditoIcmsSn { get; set; }
        public double? ValorCreditoIcmsSn { get; set; }
        public double? ValorBcIcmsStDestino { get; set; }
        public double? ValorIcmsStDestino { get; set; }
        public double? PercentualReducaoBcEfetivo { get; set; }
        public double? ValorBcEfetivo { get; set; }
        public double? AliquotaIcmsEfetivo { get; set; }
        public double? ValorIcmsEfetivo { get; set; }
    }
}

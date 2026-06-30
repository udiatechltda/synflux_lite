using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CABECALHO")]
    public class NfeCabecalho
    {
        [Key]
        public int? Id { get; set; }
        public int? IdTributOperacaoFiscal { get; set; }
        public int? UfEmitente { get; set; }
        public string? CodigoNumerico { get; set; }
        public string? NaturezaOperacao { get; set; }
        public string? CodigoModelo { get; set; }
        public string? Serie { get; set; }
        public string? Numero { get; set; }
        public DateTime? DataHoraEmissao { get; set; }
        public DateTime? DataHoraEntradaSaida { get; set; }
        public string? TipoOperacao { get; set; }
        public string? LocalDestino { get; set; }
        public int? CodigoMunicipio { get; set; }
        public string? FormatoImpressaoDanfe { get; set; }
        public string? TipoEmissao { get; set; }
        public string? ChaveAcesso { get; set; }
        public string? DigitoChaveAcesso { get; set; }
        public string? Ambiente { get; set; }
        public string? FinalidadeEmissao { get; set; }
        public string? ConsumidorOperacao { get; set; }
        public string? ConsumidorPresenca { get; set; }
        public string? ProcessoEmissao { get; set; }
        public string? VersaoProcessoEmissao { get; set; }
        public DateTime? DataEntradaContingencia { get; set; }
        public string? JustificativaContingencia { get; set; }
        public double? BaseCalculoIcms { get; set; }
        public double? ValorIcms { get; set; }
        public double? ValorIcmsDesonerado { get; set; }
        public double? TotalIcmsFcpUfDestino { get; set; }
        public double? TotalIcmsInterestadualUfDestino { get; set; }
        public double? TotalIcmsInterestadualUfRemetente { get; set; }
        public double? ValorTotalFcp { get; set; }
        public double? BaseCalculoIcmsSt { get; set; }
        public double? ValorIcmsSt { get; set; }
        public double? ValorTotalFcpSt { get; set; }
        public double? ValorTotalFcpStRetido { get; set; }
        public double? ValorTotalProdutos { get; set; }
        public double? ValorFrete { get; set; }
        public double? ValorSeguro { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorImpostoImportacao { get; set; }
        public double? ValorIpi { get; set; }
        public double? ValorIpiDevolvido { get; set; }
        public double? ValorPis { get; set; }
        public double? ValorCofins { get; set; }
        public double? ValorDespesasAcessorias { get; set; }
        public double? ValorTotal { get; set; }
        public double? ValorTotalTributos { get; set; }
        public double? ValorServicos { get; set; }
        public double? BaseCalculoIssqn { get; set; }
        public double? ValorIssqn { get; set; }
        public double? ValorPisIssqn { get; set; }
        public double? ValorCofinsIssqn { get; set; }
        public DateTime? DataPrestacaoServico { get; set; }
        public double? ValorDeducaoIssqn { get; set; }
        public double? OutrasRetencoesIssqn { get; set; }
        public double? DescontoIncondicionadoIssqn { get; set; }
        public double? DescontoCondicionadoIssqn { get; set; }
        public double? TotalRetencaoIssqn { get; set; }
        public string? RegimeEspecialTributacao { get; set; }
        public double? ValorRetidoPis { get; set; }
        public double? ValorRetidoCofins { get; set; }
        public double? ValorRetidoCsll { get; set; }
        public double? BaseCalculoIrrf { get; set; }
        public double? ValorRetidoIrrf { get; set; }
        public double? BaseCalculoPrevidencia { get; set; }
        public double? ValorRetidoPrevidencia { get; set; }
        public string? InformacoesAddFisco { get; set; }
        public string? InformacoesAddContribuinte { get; set; }
        public string? ComexUfEmbarque { get; set; }
        public string? ComexLocalEmbarque { get; set; }
        public string? ComexLocalDespacho { get; set; }
        public string? CompraNotaEmpenho { get; set; }
        public string? CompraPedido { get; set; }
        public string? CompraContrato { get; set; }
        public string? Qrcode { get; set; }
        public string? UrlChave { get; set; }
        public string? StatusNota { get; set; }
        public int? IdPdvVendaCabecalho { get; set; }
    }
}

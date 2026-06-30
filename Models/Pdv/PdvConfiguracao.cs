using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_CONFIGURACAO")]
    public class PdvConfiguracao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEcfImpressora { get; set; }
        public int? IdPdvCaixa { get; set; }
        public int? IdTributOperacaoFiscalPadrao { get; set; }
        public string? MensagemCupom { get; set; }
        public string? PortaEcf { get; set; }
        public string? IpServidor { get; set; }
        public string? IpSitef { get; set; }
        public string? TipoTef { get; set; }
        public string? TituloTelaCaixa { get; set; }
        public string? CaminhoImagensProdutos { get; set; }
        public string? CaminhoImagensMarketing { get; set; }
        public string? CorJanelasInternas { get; set; }
        public string? MarketingAtivo { get; set; }
        public int? CfopEcf { get; set; }
        public int? TimeoutEcf { get; set; }
        public int? IntervaloEcf { get; set; }
        public string? DescricaoSuprimento { get; set; }
        public string? DescricaoSangria { get; set; }
        public int? TefTipoGp { get; set; }
        public int? TefTempoEspera { get; set; }
        public int? TefEsperaSts { get; set; }
        public int? TefNumeroVias { get; set; }
        public int? DecimaisQuantidade { get; set; }
        public int? DecimaisValor { get; set; }
        public int? BitsPorSegundo { get; set; }
        public int? QuantidadeMaximaCartoes { get; set; }
        public string? PesquisaParte { get; set; }
        public string? Laudo { get; set; }
        public DateTime? DataAtualizacaoEstoque { get; set; }
        public string? PedeCpfCupom { get; set; }
        public int? TipoIntegracao { get; set; }
        public int? TimerIntegracao { get; set; }
        public string? GavetaSinalInvertido { get; set; }
        public int? GavetaUtilizacao { get; set; }
        public string? UsaTecladoReduzido { get; set; }
        public string? Modulo { get; set; }
        public string? Plano { get; set; }
        public double? PlanoValor { get; set; }
        public string? PlanoSituacao { get; set; }
        public string? ReciboFormatoPagina { get; set; }
        public double? ReciboLarguraPagina { get; set; }
        public double? ReciboMargemPagina { get; set; }
        public string? EncerraMovimentoAuto { get; set; }
        public string? PermiteEstoqueNegativo { get; set; }
        public string? ModuloFiscalPrincipal { get; set; }
        public string? ModuloFiscalContingencia { get; set; }
        public string? AcbrMonitorEndereco { get; set; }
        public int? AcbrMonitorPorta { get; set; }
    }
}

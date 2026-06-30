using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CONFIGURACAO")]
    public class NfeConfiguracao
    {
        [Key]
        public int? Id { get; set; }
        public string? CertificadoDigitalSerie { get; set; }
        public string? CertificadoDigitalCaminho { get; set; }
        public string? CertificadoDigitalSenha { get; set; }
        public int? TipoEmissao { get; set; }
        public int? FormatoImpressaoDanfe { get; set; }
        public int? ProcessoEmissao { get; set; }
        public string? VersaoProcessoEmissao { get; set; }
        public string? CaminhoLogomarca { get; set; }
        public string? SalvarXml { get; set; }
        public string? CaminhoSalvarXml { get; set; }
        public string? CaminhoSchemas { get; set; }
        public string? CaminhoArquivoDanfe { get; set; }
        public string? CaminhoSalvarPdf { get; set; }
        public string? WebserviceUf { get; set; }
        public int? WebserviceAmbiente { get; set; }
        public string? WebserviceProxyHost { get; set; }
        public int? WebserviceProxyPorta { get; set; }
        public string? WebserviceProxyUsuario { get; set; }
        public string? WebserviceProxySenha { get; set; }
        public string? WebserviceVisualizar { get; set; }
        public string? EmailServidorSmtp { get; set; }
        public int? EmailPorta { get; set; }
        public string? EmailUsuario { get; set; }
        public string? EmailSenha { get; set; }
        public string? EmailAssunto { get; set; }
        public string? EmailAutenticaSsl { get; set; }
        public string? EmailTexto { get; set; }
        public string? NfceIdCsc { get; set; }
        public string? NfceCsc { get; set; }
        public string? NfceModeloImpressao { get; set; }
        public string? NfceImprimirItensUmaLinha { get; set; }
        public string? NfceImprimirDescontoPorItem { get; set; }
        public string? NfceImprimirQrcodeLateral { get; set; }
        public string? NfceImprimirGtin { get; set; }
        public string? NfceImprimirNomeFantasia { get; set; }
        public string? NfceImpressaoTributos { get; set; }
        public double? NfceMargemSuperior { get; set; }
        public double? NfceMargemInferior { get; set; }
        public double? NfceMargemDireita { get; set; }
        public double? NfceMargemEsquerda { get; set; }
        public int? NfceResolucaoImpressao { get; set; }
        public string? RespTecCnpj { get; set; }
        public string? RespTecContato { get; set; }
        public string? RespTecEmail { get; set; }
        public string? RespTecFone { get; set; }
        public string? RespTecIdCsrt { get; set; }
        public string? RespTecHashCsrt { get; set; }
        public int? NfceTamanhoFonteItem { get; set; }
    }
}

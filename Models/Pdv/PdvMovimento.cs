using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_MOVIMENTO")]
    public class PdvMovimento
    {
        [Key]
        public int? Id { get; set; }
        public int? IdEcfImpressora { get; set; }
        public int? IdPdvOperador { get; set; }
        public int? IdPdvCaixa { get; set; }
        public int? IdGerenteSupervisor { get; set; }
        public DateTime? DataAbertura { get; set; }
        public string? HoraAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public string? HoraFechamento { get; set; }
        public double? TotalSuprimento { get; set; }
        public double? TotalSangria { get; set; }
        public double? TotalNaoFiscal { get; set; }
        public double? TotalVenda { get; set; }
        public double? TotalDesconto { get; set; }
        public double? TotalAcrescimo { get; set; }
        public double? TotalFinal { get; set; }
        public double? TotalRecebido { get; set; }
        public double? TotalTroco { get; set; }
        public double? TotalCancelado { get; set; }
        public string? StatusMovimento { get; set; }
    }
}

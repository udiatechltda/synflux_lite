using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("NFCE_PLANO_PAGAMENTO")]
    public class NfcePlanoPagamento
    {
        [Key]
        public int? Id { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? TipoPlano { get; set; }
        public double? Valor { get; set; }
        public string? StatusPagamento { get; set; }
        public string? CodigoTransacao { get; set; }
        public string? MetodoPagamento { get; set; }
        public string? CodigoTipoPagamento { get; set; }
        public DateTime? DataPlanoExpira { get; set; }
        public string? HashRegistro { get; set; }
    }
}

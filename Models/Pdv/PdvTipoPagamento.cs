using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_TIPO_PAGAMENTO")]
    public class PdvTipoPagamento
    {
        [Key]
        public int? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public string? Tef { get; set; }
        public string? ImprimeVinculado { get; set; }
        public string? PermiteTroco { get; set; }
        public string? TefTipoGp { get; set; }
        public string? GeraParcelas { get; set; }
        public string? CodigoPagamentoNfce { get; set; }
    }
}

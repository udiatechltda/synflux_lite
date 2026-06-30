using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_RECEBIMENTO_NAO_FISCAL")]
    public class EcfRecebimentoNaoFiscal
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvMovimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public string? Descricao { get; set; }
        public double? Valor { get; set; }
    }
}

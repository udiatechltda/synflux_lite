using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("TRIBUT_OPERACAO_FISCAL")]
    public class TributOperacaoFiscal
    {
        [Key]
        public int? Id { get; set; }
        public string? Descricao { get; set; }
        public string? DescricaoNaNf { get; set; }
        public int? Cfop { get; set; }
        public string? Observacao { get; set; }
    }
}

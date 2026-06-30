using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_RELATORIO_GERENCIAL")]
    public class EcfRelatorioGerencial
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvConfiguracao { get; set; }
        public int? X { get; set; }
        public int? MeiosPagamento { get; set; }
        public int? DavEmitidos { get; set; }
        public int? IdentificacaoPaf { get; set; }
        public int? ParametrosConfiguracao { get; set; }
        public int? Outros { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("TRIBUT_CONFIGURA_OF_GT")]
    public class TributConfiguraOfGt
    {
        [Key]
        public int? Id { get; set; }
        public int? IdTributGrupoTributario { get; set; }
        public int? IdTributOperacaoFiscal { get; set; }
    }
}

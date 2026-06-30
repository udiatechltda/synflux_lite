using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("TRIBUT_GRUPO_TRIBUTARIO")]
    public class TributGrupoTributario
    {
        [Key]
        public int? Id { get; set; }
        public string? Descricao { get; set; }
        public string? OrigemMercadoria { get; set; }
        public string? Observacao { get; set; }
    }
}

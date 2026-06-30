using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_CONFIGURACAO_BALANCA")]
    public class PdvConfiguracaoBalanca
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvConfiguracao { get; set; }
        public int? Modelo { get; set; }
        public string? Identificador { get; set; }
        public int? HandShake { get; set; }
        public int? Parity { get; set; }
        public int? StopBits { get; set; }
        public int? DataBits { get; set; }
        public int? BaudRate { get; set; }
        public string? Porta { get; set; }
        public int? Timeout { get; set; }
        public string? TipoConfiguracao { get; set; }
    }
}

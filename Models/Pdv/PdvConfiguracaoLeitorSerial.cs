using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_CONFIGURACAO_LEITOR_SERIAL")]
    public class PdvConfiguracaoLeitorSerial
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvConfiguracao { get; set; }
        public string? Usa { get; set; }
        public string? Porta { get; set; }
        public int? Baud { get; set; }
        public int? HandShake { get; set; }
        public int? Parity { get; set; }
        public int? StopBits { get; set; }
        public int? DataBits { get; set; }
        public int? Intervalo { get; set; }
        public string? UsarFila { get; set; }
        public string? HardFlow { get; set; }
        public string? SoftFlow { get; set; }
        public string? Sufixo { get; set; }
        public string? ExcluirSufixo { get; set; }
    }
}

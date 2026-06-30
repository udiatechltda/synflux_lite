using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_E3")]
    public class EcfE3
    {
        [Key]
        public int? Id { get; set; }
        public string? SerieEcf { get; set; }
        public string? MfAdicional { get; set; }
        public string? TipoEcf { get; set; }
        public string? MarcaEcf { get; set; }
        public string? ModeloEcf { get; set; }
        public DateTime? DataEstoque { get; set; }
        public string? HoraEstoque { get; set; }
        public string? HashRegistro { get; set; }
    }
}

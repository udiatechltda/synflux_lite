using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_NUMERO_INUTILIZADO")]
    public class NfeNumeroInutilizado
    {
        [Key]
        public int? Id { get; set; }
        public string? Serie { get; set; }
        public int? Numero { get; set; }
        public DateTime? DataInutilizacao { get; set; }
        public string? Observacao { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_ALIQUOTAS")]
    public class EcfAliquotas
    {
        [Key]
        public int? Id { get; set; }
        public string? TotalizadorParcial { get; set; }
        public string? EcfIcmsSt { get; set; }
        public string? PafPSt { get; set; }
    }
}

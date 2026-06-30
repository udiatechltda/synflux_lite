using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_NUMERO")]
    public class NfeNumero
    {
        [Key]
        public int? Id { get; set; }
        public string? Modelo { get; set; }
        public string? Serie { get; set; }
        public int? Numero { get; set; }
    }
}

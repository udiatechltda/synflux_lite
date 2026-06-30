using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_EXPORTACAO")]
    public class NfeExportacao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public int? Drawback { get; set; }
        public int? NumeroRegistro { get; set; }
        public string? ChaveAcesso { get; set; }
        public double? Quantidade { get; set; }
    }
}

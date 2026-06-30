using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_TRANSPORTE_REBOQUE")]
    public class NfeTransporteReboque
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeTransporte { get; set; }
        public string? Placa { get; set; }
        public string? Uf { get; set; }
        public string? Rntc { get; set; }
        public string? Vagao { get; set; }
        public string? Balsa { get; set; }
    }
}

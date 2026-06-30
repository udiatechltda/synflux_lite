using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_TRANSPORTE_VOLUME_LACRE")]
    public class NfeTransporteVolumeLacre
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeTransporteVolume { get; set; }
        public string? Numero { get; set; }
    }
}

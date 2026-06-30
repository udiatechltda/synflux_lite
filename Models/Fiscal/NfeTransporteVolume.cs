using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_TRANSPORTE_VOLUME")]
    public class NfeTransporteVolume
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeTransporte { get; set; }
        public int? Quantidade { get; set; }
        public string? Especie { get; set; }
        public string? Marca { get; set; }
        public string? Numeracao { get; set; }
        public double? PesoLiquido { get; set; }
        public double? PesoBruto { get; set; }
    }
}

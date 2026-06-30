using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Financeiro
{
    [Table("IBPT")]
    public class Ibpt
    {
        [Key]
        public int? Id { get; set; }
        public string? Ncm { get; set; }
        public string? Ex { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public double? NacionalFederal { get; set; }
        public double? ImportadosFederal { get; set; }
        public double? Estadual { get; set; }
        public double? Municipal { get; set; }
        public DateTime? VigenciaInicio { get; set; }
        public DateTime? VigenciaFim { get; set; }
        public string? Chave { get; set; }
        public string? Versao { get; set; }
        public string? Fonte { get; set; }
    }
}

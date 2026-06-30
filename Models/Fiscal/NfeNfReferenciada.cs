using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_NF_REFERENCIADA")]
    public class NfeNfReferenciada
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public int? CodigoUf { get; set; }
        public string? AnoMes { get; set; }
        public string? Cnpj { get; set; }
        public string? Modelo { get; set; }
        public string? Serie { get; set; }
        public int? NumeroNf { get; set; }
    }
}

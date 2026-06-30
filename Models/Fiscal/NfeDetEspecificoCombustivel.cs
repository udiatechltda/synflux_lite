using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DET_ESPECIFICO_COMBUSTIVEL")]
    public class NfeDetEspecificoCombustivel
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public int? CodigoAnp { get; set; }
        public string? DescricaoAnp { get; set; }
        public double? PercentualGlp { get; set; }
        public double? PercentualGasNacional { get; set; }
        public double? PercentualGasImportado { get; set; }
        public double? ValorPartida { get; set; }
        public string? Codif { get; set; }
        public double? QuantidadeTempAmbiente { get; set; }
        public string? UfConsumo { get; set; }
        public double? CideBaseCalculo { get; set; }
        public double? CideAliquota { get; set; }
        public double? CideValor { get; set; }
        public int? EncerranteBico { get; set; }
        public int? EncerranteBomba { get; set; }
        public int? EncerranteTanque { get; set; }
        public double? EncerranteValorInicio { get; set; }
        public double? EncerranteValorFim { get; set; }
    }
}

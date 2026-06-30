using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DET_ESPECIFICO_VEICULO")]
    public class NfeDetEspecificoVeiculo
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? TipoOperacao { get; set; }
        public string? Chassi { get; set; }
        public string? Cor { get; set; }
        public string? DescricaoCor { get; set; }
        public string? PotenciaMotor { get; set; }
        public string? Cilindradas { get; set; }
        public string? PesoLiquido { get; set; }
        public string? PesoBruto { get; set; }
        public string? NumeroSerie { get; set; }
        public string? TipoCombustivel { get; set; }
        public string? NumeroMotor { get; set; }
        public string? CapacidadeMaximaTracao { get; set; }
        public string? DistanciaEixos { get; set; }
        public string? AnoModelo { get; set; }
        public string? AnoFabricacao { get; set; }
        public string? TipoPintura { get; set; }
        public string? TipoVeiculo { get; set; }
        public string? EspecieVeiculo { get; set; }
        public string? CondicaoVin { get; set; }
        public string? CondicaoVeiculo { get; set; }
        public string? CodigoMarcaModelo { get; set; }
        public string? CodigoCorDenatran { get; set; }
        public int? LotacaoMaxima { get; set; }
        public string? Restricao { get; set; }
    }
}

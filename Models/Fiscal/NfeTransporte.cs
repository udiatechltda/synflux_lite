using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_TRANSPORTE")]
    public class NfeTransporte
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? ModalidadeFrete { get; set; }
        public string? Cnpj { get; set; }
        public string? Cpf { get; set; }
        public string? Nome { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? Endereco { get; set; }
        public string? NomeMunicipio { get; set; }
        public string? Uf { get; set; }
        public double? ValorServico { get; set; }
        public double? ValorBcRetencaoIcms { get; set; }
        public double? AliquotaRetencaoIcms { get; set; }
        public double? ValorIcmsRetido { get; set; }
        public int? Cfop { get; set; }
        public int? Municipio { get; set; }
        public string? PlacaVeiculo { get; set; }
        public string? UfVeiculo { get; set; }
        public string? RntcVeiculo { get; set; }
    }
}

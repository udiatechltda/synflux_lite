using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_R01")]
    public class EcfR01
    {
        [Key]
        public int? Id { get; set; }
        public string? SerieEcf { get; set; }
        public string? CnpjEmpresa { get; set; }
        public string? CnpjSh { get; set; }
        public string? InscricaoEstadualSh { get; set; }
        public string? InscricaoMunicipalSh { get; set; }
        public string? DenominacaoSh { get; set; }
        public string? NomePafEcf { get; set; }
        public string? VersaoPafEcf { get; set; }
        public string? Md5PafEcf { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public string? VersaoEr { get; set; }
        public string? NumeroLaudoPaf { get; set; }
        public string? RazaoSocialSh { get; set; }
        public string? EnderecoSh { get; set; }
        public string? NumeroSh { get; set; }
        public string? ComplementoSh { get; set; }
        public string? BairroSh { get; set; }
        public string? CidadeSh { get; set; }
        public string? CepSh { get; set; }
        public string? UfSh { get; set; }
        public string? TelefoneSh { get; set; }
        public string? ContatoSh { get; set; }
        public string? PrincipalExecutavel { get; set; }
        public string? HashRegistro { get; set; }
    }
}

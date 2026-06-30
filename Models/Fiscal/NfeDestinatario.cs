using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DESTINATARIO")]
    public class NfeDestinatario
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? Cnpj { get; set; }
        public string? Cpf { get; set; }
        public string? EstrangeiroIdentificacao { get; set; }
        public string? Nome { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public int? CodigoMunicipio { get; set; }
        public string? NomeMunicipio { get; set; }
        public string? Uf { get; set; }
        public string? Cep { get; set; }
        public int? CodigoPais { get; set; }
        public string? NomePais { get; set; }
        public string? Telefone { get; set; }
        public string? IndicadorIe { get; set; }
        public string? InscricaoEstadual { get; set; }
        public int? Suframa { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Email { get; set; }
    }
}

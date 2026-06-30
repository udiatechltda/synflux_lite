using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Fantasia { get; set; }
        public string? Email { get; set; }
        public string? Url { get; set; }
        public string? CpfCnpj { get; set; }
        public string? Rg { get; set; }
        public string? OrgaoRg { get; set; }
        public DateTime? DataEmissaoRg { get; set; }
        public string? Sexo { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? TipoPessoa { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Contato { get; set; }
        public int? CodigoIbgeCidade { get; set; }
        public int? CodigoIbgeUf { get; set; }
        public string? FidelidadeAviso { get; set; }
        public int? FidelidadeQuantidade { get; set; }
        public double? FidelidadeValor { get; set; }
        public double? FiadoValorTeto { get; set; }
    }
}

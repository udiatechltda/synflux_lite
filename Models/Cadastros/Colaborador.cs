using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("COLABORADOR")]
    public class Colaborador
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public double? ComissaoVista { get; set; }
        public double? ComissaoPrazo { get; set; }
        public string? NivelAutorizacao { get; set; }
        public string? EntregadorVeiculo { get; set; }
    }
}

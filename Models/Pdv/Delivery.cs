using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("DELIVERY")]
    public class Delivery
    {
        [Key]
        public int? Id { get; set; }
        public int? IdComanda { get; set; }
        public int? IdTaxaEntrega { get; set; }
        public int? IdColaborador { get; set; }
        public string? NomeCliente { get; set; }
        public string? TelefonePrincipal { get; set; }
        public string? TelefoneRecado { get; set; }
        public string? Celular { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cep { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public double? ValorFrete { get; set; }
        public double? ValorRecebido { get; set; }
        public double? ValorAReceber { get; set; }
        public double? ValorSolicitadoTroco { get; set; }
        public DateTime? PrevisaoPreparo { get; set; }
        public DateTime? InicioPreparo { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public DateTime? SaiuParaEntrega { get; set; }
        public DateTime? Entregue { get; set; }
        public DateTime? PrevisaoRetirada { get; set; }
        public DateTime? ProntoParaRetirada { get; set; }
        public DateTime? Retirou { get; set; }
    }
}

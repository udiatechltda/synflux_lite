using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("CARDAPIO")]
    public class Cardapio
    {
        [Key]
        public int? Id { get; set; }
        public int? IdProduto { get; set; }
        public string? ModoPreparo { get; set; }
        public string? InfoAlergico { get; set; }
        public string? Ingredientes { get; set; }
    }
}

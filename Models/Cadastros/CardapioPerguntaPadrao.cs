using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("CARDAPIO_PERGUNTA_PADRAO")]
    public class CardapioPerguntaPadrao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCardapio { get; set; }
        public string? Pergunta { get; set; }
    }
}

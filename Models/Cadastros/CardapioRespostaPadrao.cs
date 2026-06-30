using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("CARDAPIO_RESPOSTA_PADRAO")]
    public class CardapioRespostaPadrao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdCardapioPerguntaPadrao { get; set; }
        public string? Resposta { get; set; }
    }
}

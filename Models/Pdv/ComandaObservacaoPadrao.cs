using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("COMANDA_OBSERVACAO_PADRAO")]
    public class ComandaObservacaoPadrao
    {
        [Key]
        public int? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
    }
}

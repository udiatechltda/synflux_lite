using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_CAIXA")]
    public class PdvCaixa
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}

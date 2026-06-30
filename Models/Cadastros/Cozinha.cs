using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Cadastros
{
    [Table("COZINHA")]
    public class Cozinha
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? ImpressoraNome { get; set; }
        public string? ImpressoraEndereco { get; set; }
    }
}

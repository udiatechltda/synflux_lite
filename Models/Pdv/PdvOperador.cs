using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv
{
    [Table("PDV_OPERADOR")]
    public class PdvOperador
    {
        [Key]
        public int? Id { get; set; }
        public int? IdColaborador { get; set; }
        public string? Login { get; set; }
        public string? Senha { get; set; }
    }
}

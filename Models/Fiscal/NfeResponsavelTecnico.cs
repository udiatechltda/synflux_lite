using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_RESPONSAVEL_TECNICO")]
    public class NfeResponsavelTecnico
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? Cnpj { get; set; }
        public string? Contato { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? IdentificadorCsrt { get; set; }
        public string? HashCsrt { get; set; }
    }
}

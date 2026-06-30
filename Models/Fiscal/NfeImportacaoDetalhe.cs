using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_IMPORTACAO_DETALHE")]
    public class NfeImportacaoDetalhe
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDeclaracaoImportacao { get; set; }
        public int? NumeroAdicao { get; set; }
        public int? NumeroSequencial { get; set; }
        public string? CodigoFabricanteEstrangeiro { get; set; }
        public double? ValorDesconto { get; set; }
        public int? Drawback { get; set; }
    }
}

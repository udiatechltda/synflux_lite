using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_DECLARACAO_IMPORTACAO")]
    public class NfeDeclaracaoImportacao
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeDetalhe { get; set; }
        public string? NumeroDocumento { get; set; }
        public DateTime? DataRegistro { get; set; }
        public string? LocalDesembaraco { get; set; }
        public string? UfDesembaraco { get; set; }
        public DateTime? DataDesembaraco { get; set; }
        public string? ViaTransporte { get; set; }
        public double? ValorAfrmm { get; set; }
        public string? FormaIntermediacao { get; set; }
        public string? Cnpj { get; set; }
        public string? UfTerceiro { get; set; }
        public string? CodigoExportador { get; set; }
    }
}

using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_SINTEGRA_60M")]
    public class EcfSintegra60M
    {
        [Key]
        public int? Id { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string? NumeroSerieEcf { get; set; }
        public int? NumeroEquipamento { get; set; }
        public string? ModeloDocumentoFiscal { get; set; }
        public int? CooInicial { get; set; }
        public int? CooFinal { get; set; }
        public int? Crz { get; set; }
        public int? Cro { get; set; }
        public double? ValorVendaBruta { get; set; }
        public double? ValorGrandeTotal { get; set; }
    }
}

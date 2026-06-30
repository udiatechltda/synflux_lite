using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CUPOM_FISCAL_REFERENCIADO")]
    public class NfeCupomFiscalReferenciado
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? ModeloDocumentoFiscal { get; set; }
        public int? NumeroOrdemEcf { get; set; }
        public int? Coo { get; set; }
        public DateTime? DataEmissaoCupom { get; set; }
        public int? NumeroCaixa { get; set; }
        public string? NumeroSerieEcf { get; set; }
    }
}

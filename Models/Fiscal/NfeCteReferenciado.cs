using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("NFE_CTE_REFERENCIADO")]
    public class NfeCteReferenciado
    {
        [Key]
        public int? Id { get; set; }
        public int? IdNfeCabecalho { get; set; }
        public string? ChaveAcesso { get; set; }
    }
}

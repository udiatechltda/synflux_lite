using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV.Models.Pdv.Fiscal
{
    [Table("ECF_DOCUMENTOS_EMITIDOS")]
    public class EcfDocumentosEmitidos
    {
        [Key]
        public int? Id { get; set; }
        public int? IdPdvMovimento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string? HoraEmissao { get; set; }
        public string? Tipo { get; set; }
        public int? Coo { get; set; }
    }
}

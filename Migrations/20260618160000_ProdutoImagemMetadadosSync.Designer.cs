using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PDV.Services;

#nullable disable

namespace PDV.Migrations
{
    [DbContext(typeof(PdvContext))]
    [Migration("20260618160000_ProdutoImagemMetadadosSync")]
    partial class ProdutoImagemMetadadosSync
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
        }
    }
}

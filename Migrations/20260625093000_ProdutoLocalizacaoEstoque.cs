using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PDV.Services;

#nullable disable

namespace PDV.Migrations
{
    [DbContext(typeof(PdvContext))]
    [Migration("20260625093000_ProdutoLocalizacaoEstoque")]
    public partial class ProdutoLocalizacaoEstoque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Localizacao",
                table: "PRODUTO",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Localizacao",
                table: "PRODUTO");
        }
    }
}

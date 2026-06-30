using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDV.Migrations
{
    public partial class ProdutoImagemMetadadosSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AtualizadoEm",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HashSha256",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PendenteExclusao",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PendenteUpload",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SincronizadoEm",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TamanhoBytes",
                table: "PRODUTO_IMAGEM",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlRemota",
                table: "PRODUTO_IMAGEM",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtualizadoEm",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "HashSha256",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "PendenteExclusao",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "PendenteUpload",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "SincronizadoEm",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "TamanhoBytes",
                table: "PRODUTO_IMAGEM");

            migrationBuilder.DropColumn(
                name: "UrlRemota",
                table: "PRODUTO_IMAGEM");
        }
    }
}

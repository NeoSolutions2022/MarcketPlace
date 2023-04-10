using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddPropriedadesDeAnuncioParaProdutoEFornecedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnuncioPago",
                table: "ProdutoServicos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpiracaoAnuncio",
                table: "ProdutoServicos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamentoAnuncio",
                table: "ProdutoServicos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AnuncioPago",
                table: "Fornecedores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpiracaoAnuncio",
                table: "Fornecedores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamentoAnuncio",
                table: "Fornecedores",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnuncioPago",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "DataExpiracaoAnuncio",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "DataPagamentoAnuncio",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "AnuncioPago",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "DataExpiracaoAnuncio",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "DataPagamentoAnuncio",
                table: "Fornecedores");
        }
    }
}

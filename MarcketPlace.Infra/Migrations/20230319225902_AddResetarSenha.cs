using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddResetarSenha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoResetarSenha",
                table: "Fornecedores",
                type: "CHAR(64)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Fornecedores",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoResetarSenha",
                table: "Clientes",
                type: "CHAR(64)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Clientes",
                type: "DATETIME",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoResetarSenha",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "CodigoResetarSenha",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Clientes");
        }
    }
}

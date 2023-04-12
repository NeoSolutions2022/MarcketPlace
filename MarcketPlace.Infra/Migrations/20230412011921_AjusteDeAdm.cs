using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AjusteDeAdm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoResetarSenha",
                table: "Administradores",
                type: "CHAR(64)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Administradores",
                type: "DATETIME",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoResetarSenha",
                table: "Administradores");

            migrationBuilder.DropColumn(
                name: "CodigoResetarSenhaExpiraEm",
                table: "Administradores");
        }
    }
}

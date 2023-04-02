using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddFotoFornecedorECliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Fornecedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Clientes");
        }
    }
}

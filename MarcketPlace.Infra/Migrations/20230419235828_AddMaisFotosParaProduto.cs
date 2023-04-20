using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddMaisFotosParaProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto2",
                table: "ProdutoServicos",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto3",
                table: "ProdutoServicos",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto4",
                table: "ProdutoServicos",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Foto5",
                table: "ProdutoServicos",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto2",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "Foto3",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "Foto4",
                table: "ProdutoServicos");

            migrationBuilder.DropColumn(
                name: "Foto5",
                table: "ProdutoServicos");
        }
    }
}

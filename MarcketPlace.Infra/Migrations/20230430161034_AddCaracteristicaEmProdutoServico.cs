using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddCaracteristicaEmProdutoServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caracteristica",
                table: "ProdutoServicos",
                type: "nvarchar(max)",
                maxLength: 8000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caracteristica",
                table: "ProdutoServicos");
        }
    }
}

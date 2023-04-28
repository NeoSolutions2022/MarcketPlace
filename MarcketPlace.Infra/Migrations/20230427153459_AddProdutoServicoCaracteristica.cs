using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcketPlace.Infra.Migrations
{
    public partial class AddProdutoServicoCaracteristica : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdutoServicoCaracteristicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Chave = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProdutoServicoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoServicoCaracteristicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoServicoCaracteristicas_ProdutoServicos_ProdutoServicoId",
                        column: x => x.ProdutoServicoId,
                        principalTable: "ProdutoServicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoServicoCaracteristicas_ProdutoServicoId",
                table: "ProdutoServicoCaracteristicas",
                column: "ProdutoServicoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoServicoCaracteristicas");
        }
    }
}

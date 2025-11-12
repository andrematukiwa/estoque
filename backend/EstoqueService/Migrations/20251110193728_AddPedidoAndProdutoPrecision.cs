using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueService.Migrations
{
    /// <inheritdoc />
    public partial class AddPedidoAndProdutoPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "TB_PRODUTO");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TB_PRODUTO",
                newName: "ID_PRODUTO");

            migrationBuilder.RenameColumn(
                name: "QuantidadeEmEstoque",
                table: "TB_PRODUTO",
                newName: "QTD_STOCK");

            migrationBuilder.RenameColumn(
                name: "PrecoUnitario",
                table: "TB_PRODUTO",
                newName: "NUM_UNIT_PRICE");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "TB_PRODUTO",
                newName: "DSC_NAME");

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "TB_PRODUTO",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_PRODUTO",
                table: "TB_PRODUTO",
                column: "ID_PRODUTO");

            migrationBuilder.CreateTable(
                name: "TB_PEDIDO",
                columns: table => new
                {
                    ID_PEDIDO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DT_PEDIDO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DSC_CLIENTE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NUM_VALOR_TOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PEDIDO", x => x.ID_PEDIDO);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_PRODUTO_PedidoId",
                table: "TB_PRODUTO",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_PedidoId",
                table: "TB_PRODUTO",
                column: "PedidoId",
                principalTable: "TB_PEDIDO",
                principalColumn: "ID_PEDIDO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_PedidoId",
                table: "TB_PRODUTO");

            migrationBuilder.DropTable(
                name: "TB_PEDIDO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_PRODUTO",
                table: "TB_PRODUTO");

            migrationBuilder.DropIndex(
                name: "IX_TB_PRODUTO_PedidoId",
                table: "TB_PRODUTO");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "TB_PRODUTO");

            migrationBuilder.RenameTable(
                name: "TB_PRODUTO",
                newName: "Produtos");

            migrationBuilder.RenameColumn(
                name: "ID_PRODUTO",
                table: "Produtos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QTD_STOCK",
                table: "Produtos",
                newName: "QuantidadeEmEstoque");

            migrationBuilder.RenameColumn(
                name: "NUM_UNIT_PRICE",
                table: "Produtos",
                newName: "PrecoUnitario");

            migrationBuilder.RenameColumn(
                name: "DSC_NAME",
                table: "Produtos",
                newName: "Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");
        }
    }
}

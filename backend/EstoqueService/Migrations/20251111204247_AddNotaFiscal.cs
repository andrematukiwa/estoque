using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueService.Migrations
{
    /// <inheritdoc />
    public partial class AddNotaFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_PedidoId",
                table: "TB_PRODUTO");

            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "TB_PRODUTO",
                newName: "ID_PEDIDO");

            migrationBuilder.RenameIndex(
                name: "IX_TB_PRODUTO_PedidoId",
                table: "TB_PRODUTO",
                newName: "IX_TB_PRODUTO_ID_PEDIDO");

            migrationBuilder.AddColumn<int>(
                name: "ID_NOTA_FISCAL",
                table: "TB_PRODUTO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DSC_STATUS",
                table: "TB_PEDIDO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NUM_NOTA_FISCAL",
                table: "TB_PEDIDO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TB_NOTA_FISCAL",
                columns: table => new
                {
                    ID_NOTA_FISCAL = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUM_NOTA_FISCAL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DSC_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DT_EMISSAO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NUM_VALOR_TOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NOTA_FISCAL", x => x.ID_NOTA_FISCAL);
                    table.ForeignKey(
                        name: "FK_TB_NOTA_FISCAL_TB_PEDIDO_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "TB_PEDIDO",
                        principalColumn: "ID_PEDIDO");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_PRODUTO_ID_NOTA_FISCAL",
                table: "TB_PRODUTO",
                column: "ID_NOTA_FISCAL");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTA_FISCAL_PedidoId",
                table: "TB_NOTA_FISCAL",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_PRODUTO_TB_NOTA_FISCAL_ID_NOTA_FISCAL",
                table: "TB_PRODUTO",
                column: "ID_NOTA_FISCAL",
                principalTable: "TB_NOTA_FISCAL",
                principalColumn: "ID_NOTA_FISCAL");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_ID_PEDIDO",
                table: "TB_PRODUTO",
                column: "ID_PEDIDO",
                principalTable: "TB_PEDIDO",
                principalColumn: "ID_PEDIDO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_PRODUTO_TB_NOTA_FISCAL_ID_NOTA_FISCAL",
                table: "TB_PRODUTO");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_ID_PEDIDO",
                table: "TB_PRODUTO");

            migrationBuilder.DropTable(
                name: "TB_NOTA_FISCAL");

            migrationBuilder.DropIndex(
                name: "IX_TB_PRODUTO_ID_NOTA_FISCAL",
                table: "TB_PRODUTO");

            migrationBuilder.DropColumn(
                name: "ID_NOTA_FISCAL",
                table: "TB_PRODUTO");

            migrationBuilder.DropColumn(
                name: "DSC_STATUS",
                table: "TB_PEDIDO");

            migrationBuilder.DropColumn(
                name: "NUM_NOTA_FISCAL",
                table: "TB_PEDIDO");

            migrationBuilder.RenameColumn(
                name: "ID_PEDIDO",
                table: "TB_PRODUTO",
                newName: "PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_TB_PRODUTO_ID_PEDIDO",
                table: "TB_PRODUTO",
                newName: "IX_TB_PRODUTO_PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_PRODUTO_TB_PEDIDO_PedidoId",
                table: "TB_PRODUTO",
                column: "PedidoId",
                principalTable: "TB_PEDIDO",
                principalColumn: "ID_PEDIDO");
        }
    }
}

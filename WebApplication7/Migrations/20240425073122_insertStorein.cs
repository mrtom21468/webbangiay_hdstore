using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class insertStorein : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "State",
                table: "Product",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "StoreIns",
                columns: table => new
                {
                    StoreInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductdetailId = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    ImprortPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreIns", x => x.StoreInId);
                    table.ForeignKey(
                        name: "FK_StoreIns_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "FK_StoreIns_ProductDetail_ProductdetailId",
                        column: x => x.ProductdetailId,
                        principalTable: "ProductDetail",
                        principalColumn: "productdetail_id");
                    table.ForeignKey(
                        name: "FK_StoreIns_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "supplier_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreIns_AccountId",
                table: "StoreIns",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIns_ProductdetailId",
                table: "StoreIns",
                column: "ProductdetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIns_SupplierId",
                table: "StoreIns",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreIns");

            migrationBuilder.AlterColumn<bool>(
                name: "State",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class Updateproductorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Product__supplie__44FF419A",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "supplier_id",
                table: "Product",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_supplier_id",
                table: "Product",
                newName: "IX_Product_SupplierId");

            migrationBuilder.AddColumn<decimal>(
                name: "importPrice",
                table: "ProductDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtOrderTime",
                table: "OrderDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "supplier_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "importPrice",
                table: "ProductDetail");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PriceAtOrderTime",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Product",
                newName: "supplier_id");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SupplierId",
                table: "Product",
                newName: "IX_Product_supplier_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Product__supplie__44FF419A",
                table: "Product",
                column: "supplier_id",
                principalTable: "Supplier",
                principalColumn: "supplier_id");
        }
    }
}

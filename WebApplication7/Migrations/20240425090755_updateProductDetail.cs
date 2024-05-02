using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class updateProductDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "purchase_price",
                table: "Product");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "ProductDetail",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "ProductDetail");

            migrationBuilder.AddColumn<decimal>(
                name: "purchase_price",
                table: "Product",
                type: "decimal(10,2)",
                nullable: true);
        }
    }
}

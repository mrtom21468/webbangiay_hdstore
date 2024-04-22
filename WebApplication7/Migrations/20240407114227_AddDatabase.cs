using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication7.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wallet_id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    user_id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__46A222CD690D75C5", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    brand_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Brand__5E5A8E2750A69CA3", x => x.brand_id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__D54EE9B4FAB19B5D", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    color_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    color_name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Color__1143CECB9627DBA7", x => x.color_id);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    size_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    size_name = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    SizeCode = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Size__0DCACE3135AEF154", x => x.size_id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__6EE594E8AE47FBB9", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    full_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Address__CAA247C852F9A3C0", x => x.address_id);
                    table.ForeignKey(
                        name: "FK__Address__account__5BE2A6F2",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_id = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    payment_status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "cancelled"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__465962296DF7035F", x => x.order_id);
                    table.ForeignKey(
                        name: "FK__Order__account_i__5535A963",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    supplier_id = table.Column<int>(type: "int", nullable: true),
                    purchase_price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__47027DF5F3E00BD2", x => x.product_id);
                    table.ForeignKey(
                        name: "FK__Product__brand_i__4316F928",
                        column: x => x.brand_id,
                        principalTable: "Brand",
                        principalColumn: "brand_id");
                    table.ForeignKey(
                        name: "FK__Product__categor__440B1D61",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK__Product__supplie__44FF419A",
                        column: x => x.supplier_id,
                        principalTable: "Supplier",
                        principalColumn: "supplier_id");
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    photo_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    photo_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    photo_url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Photo__CB48C83D51872FDC", x => x.photo_id);
                    table.ForeignKey(
                        name: "FK__Photo__product_i__47DBAE45",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    productdetail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    size_id = table.Column<int>(type: "int", nullable: true),
                    color_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    selling_price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductD__A823340D55986CEB", x => x.productdetail_id);
                    table.ForeignKey(
                        name: "FK__ProductDe__color__4CA06362",
                        column: x => x.color_id,
                        principalTable: "Color",
                        principalColumn: "color_id");
                    table.ForeignKey(
                        name: "FK__ProductDe__produ__4AB81AF0",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "FK__ProductDe__size___4BAC3F29",
                        column: x => x.size_id,
                        principalTable: "Size",
                        principalColumn: "size_id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    productdetail_id = table.Column<int>(type: "int", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__6C491621872E96F5", x => new { x.productdetail_id, x.account_id });
                    table.ForeignKey(
                        name: "FK__Cart__account_id__5070F446",
                        column: x => x.account_id,
                        principalTable: "Account",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "FK__Cart__productdet__4F7CD00D",
                        column: x => x.productdetail_id,
                        principalTable: "ProductDetail",
                        principalColumn: "productdetail_id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    order_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    productdetail_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3C5A4080CB8FC91C", x => x.order_detail_id);
                    table.ForeignKey(
                        name: "FK__OrderDeta__order__5812160E",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "order_id");
                    table.ForeignKey(
                        name: "FK__OrderDeta__produ__59063A47",
                        column: x => x.productdetail_id,
                        principalTable: "ProductDetail",
                        principalColumn: "productdetail_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_account_id",
                table: "Address",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_account_id",
                table: "Cart",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_account_id",
                table: "Order",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_order_id",
                table: "OrderDetail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_productdetail_id",
                table: "OrderDetail",
                column: "productdetail_id");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_product_id",
                table: "Photo",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_brand_id",
                table: "Product",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_supplier_id",
                table: "Product",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_color_id",
                table: "ProductDetail",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_product_id",
                table: "ProductDetail",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_size_id",
                table: "ProductDetail",
                column: "size_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Supplier");
        }
    }
}

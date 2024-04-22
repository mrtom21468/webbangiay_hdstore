﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication7.Models;

#nullable disable

namespace WebApplication7.Migrations
{
    [DbContext(typeof(QLDBcontext))]
    partial class QLDBcontextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("user_id");

                    b.Property<string>("WalletId")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("wallet_id");

                    b.HasKey("AccountId")
                        .HasName("PK__Account__46A222CD690D75C5");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("country");

                    b.Property<string>("FullAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("full_address");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone_number");

                    b.HasKey("AddressId")
                        .HasName("PK__Address__CAA247C852F9A3C0");

                    b.HasIndex(new[] { "AccountId" }, "IX_Address_account_id");

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("brand_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("brand_name");

                    b.HasKey("BrandId")
                        .HasName("PK__Brand__5E5A8E2750A69CA3");

                    b.ToTable("Brand", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Cart", b =>
                {
                    b.Property<int>("ProductdetailId")
                        .HasColumnType("int")
                        .HasColumnName("productdetail_id");

                    b.Property<int>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasColumnName("amount");

                    b.HasKey("ProductdetailId", "AccountId")
                        .HasName("PK__Cart__6C491621872E96F5");

                    b.HasIndex(new[] { "AccountId" }, "IX_Cart_account_id");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("category_name");

                    b.Property<string>("CategorySlug")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId")
                        .HasName("PK__Category__D54EE9B4FAB19B5D");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("color_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColorId"));

                    b.Property<string>("ColorName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)")
                        .HasColumnName("color_name");

                    b.HasKey("ColorId")
                        .HasName("PK__Color__1143CECB9627DBA7");

                    b.ToTable("Color", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int")
                        .HasColumnName("account_id");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("address");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("PaymentStatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValue("cancelled")
                        .HasColumnName("payment_status");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone_number");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total_amount");

                    b.HasKey("OrderId")
                        .HasName("PK__Order__465962296DF7035F");

                    b.HasIndex(new[] { "AccountId" }, "IX_Order_account_id");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_detail_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    b.Property<int?>("ProductdetailId")
                        .HasColumnType("int")
                        .HasColumnName("productdetail_id");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.HasKey("OrderDetailId")
                        .HasName("PK__OrderDet__3C5A4080CB8FC91C");

                    b.HasIndex(new[] { "OrderId" }, "IX_OrderDetail_order_id");

                    b.HasIndex(new[] { "ProductdetailId" }, "IX_OrderDetail_productdetail_id");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("photo_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhotoId"));

                    b.Property<string>("PhotoName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("photo_name");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("photo_url");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.HasKey("PhotoId")
                        .HasName("PK__Photo__CB48C83D51872FDC");

                    b.HasIndex(new[] { "ProductId" }, "IX_Photo_product_id");

                    b.ToTable("Photo", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int?>("BrandId")
                        .HasColumnType("int")
                        .HasColumnName("brand_id");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("product_name");

                    b.Property<decimal?>("PurchasePrice")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("purchase_price");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("int")
                        .HasColumnName("supplier_id");

                    b.HasKey("ProductId")
                        .HasName("PK__Product__47027DF5F3E00BD2");

                    b.HasIndex(new[] { "BrandId" }, "IX_Product_brand_id");

                    b.HasIndex(new[] { "CategoryId" }, "IX_Product_category_id");

                    b.HasIndex(new[] { "SupplierId" }, "IX_Product_supplier_id");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.ProductDetail", b =>
                {
                    b.Property<int>("ProductdetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("productdetail_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductdetailId"));

                    b.Property<int?>("ColorId")
                        .HasColumnType("int")
                        .HasColumnName("color_id");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<decimal?>("SellingPrice")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("selling_price");

                    b.Property<int?>("SizeId")
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    b.HasKey("ProductdetailId")
                        .HasName("PK__ProductD__A823340D55986CEB");

                    b.HasIndex(new[] { "ColorId" }, "IX_ProductDetail_color_id");

                    b.HasIndex(new[] { "ProductId" }, "IX_ProductDetail_product_id");

                    b.HasIndex(new[] { "SizeId" }, "IX_ProductDetail_size_id");

                    b.ToTable("ProductDetail", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<string>("SizeCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("SizeName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("size_name");

                    b.HasKey("SizeId")
                        .HasName("PK__Size__0DCACE3135AEF154");

                    b.ToTable("Size", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("supplier_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierId"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("address");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone_number");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("supplier_name");

                    b.HasKey("SupplierId")
                        .HasName("PK__Supplier__6EE594E8AE47FBB9");

                    b.ToTable("Supplier", (string)null);
                });

            modelBuilder.Entity("WebApplication7.Models.UserIdentitycs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebApplication7.Models.UserIdentitycs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebApplication7.Models.UserIdentitycs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication7.Models.UserIdentitycs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebApplication7.Models.UserIdentitycs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication7.Models.Address", b =>
                {
                    b.HasOne("WebApplication7.Models.Account", "Account")
                        .WithMany("Addresses")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Address__account__5BE2A6F2");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("WebApplication7.Models.Cart", b =>
                {
                    b.HasOne("WebApplication7.Models.Account", "Account")
                        .WithMany("Carts")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("FK__Cart__account_id__5070F446");

                    b.HasOne("WebApplication7.Models.ProductDetail", "Productdetail")
                        .WithMany("Carts")
                        .HasForeignKey("ProductdetailId")
                        .IsRequired()
                        .HasConstraintName("FK__Cart__productdet__4F7CD00D");

                    b.Navigation("Account");

                    b.Navigation("Productdetail");
                });

            modelBuilder.Entity("WebApplication7.Models.Order", b =>
                {
                    b.HasOne("WebApplication7.Models.Account", "Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Order__account_i__5535A963");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("WebApplication7.Models.OrderDetail", b =>
                {
                    b.HasOne("WebApplication7.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK__OrderDeta__order__5812160E");

                    b.HasOne("WebApplication7.Models.ProductDetail", "Productdetail")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductdetailId")
                        .HasConstraintName("FK__OrderDeta__produ__59063A47");

                    b.Navigation("Order");

                    b.Navigation("Productdetail");
                });

            modelBuilder.Entity("WebApplication7.Models.Photo", b =>
                {
                    b.HasOne("WebApplication7.Models.Product", "Product")
                        .WithMany("Photos")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__Photo__product_i__47DBAE45");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebApplication7.Models.Product", b =>
                {
                    b.HasOne("WebApplication7.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .HasConstraintName("FK__Product__brand_i__4316F928");

                    b.HasOne("WebApplication7.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__Product__categor__440B1D61");

                    b.HasOne("WebApplication7.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .HasConstraintName("FK__Product__supplie__44FF419A");

                    b.Navigation("Brand");

                    b.Navigation("Category");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("WebApplication7.Models.ProductDetail", b =>
                {
                    b.HasOne("WebApplication7.Models.Color", "Color")
                        .WithMany("ProductDetails")
                        .HasForeignKey("ColorId")
                        .HasConstraintName("FK__ProductDe__color__4CA06362");

                    b.HasOne("WebApplication7.Models.Product", "Product")
                        .WithMany("ProductDetails")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__ProductDe__produ__4AB81AF0");

                    b.HasOne("WebApplication7.Models.Size", "Size")
                        .WithMany("ProductDetails")
                        .HasForeignKey("SizeId")
                        .HasConstraintName("FK__ProductDe__size___4BAC3F29");

                    b.Navigation("Color");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("WebApplication7.Models.Account", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("WebApplication7.Models.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebApplication7.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebApplication7.Models.Color", b =>
                {
                    b.Navigation("ProductDetails");
                });

            modelBuilder.Entity("WebApplication7.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("WebApplication7.Models.Product", b =>
                {
                    b.Navigation("Photos");

                    b.Navigation("ProductDetails");
                });

            modelBuilder.Entity("WebApplication7.Models.ProductDetail", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("WebApplication7.Models.Size", b =>
                {
                    b.Navigation("ProductDetails");
                });

            modelBuilder.Entity("WebApplication7.Models.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace WebApplication7.Models
{
    public class QLDBcontext : IdentityDbContext<UserIdentitycs>
    {
        public QLDBcontext(DbContextOptions<QLDBcontext> options) :base(options) { }
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Photo> Photos { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductDetail> ProductDetails { get; set; }

        public virtual DbSet<Size> Sizes { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CD690D75C5");

                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_id");
                entity.Property(e => e.WalletId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("wallet_id");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Address__CAA247C852F9A3C0");

                entity.ToTable("Address");

                entity.HasIndex(e => e.AccountId, "IX_Address_account_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");
                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .HasColumnName("country");
                entity.Property(e => e.FullAddress)
                    .HasMaxLength(255)
                    .HasColumnName("full_address");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");

                entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Address__account__5BE2A6F2");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.BrandId).HasName("PK__Brand__5E5A8E2750A69CA3");

                entity.ToTable("Brand");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(100)
                    .HasColumnName("brand_name");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => new { e.ProductdetailId, e.AccountId }).HasName("PK__Cart__6C491621872E96F5");

                entity.ToTable("Cart");

                entity.HasIndex(e => e.AccountId, "IX_Cart_account_id");

                entity.Property(e => e.ProductdetailId).HasColumnName("productdetail_id");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__account_id__5070F446");

                entity.HasOne(d => d.Productdetail).WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductdetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cart__productdet__4F7CD00D");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4FAB19B5D");

                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId).HasName("PK__Color__1143CECB9627DBA7");

                entity.ToTable("Color");

                entity.Property(e => e.ColorId).HasColumnName("color_id");
                entity.Property(e => e.ColorName)
                    .HasMaxLength(15)
                    .HasColumnName("color_name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Order__465962296DF7035F");

                entity.ToTable("Order");

                entity.HasIndex(e => e.AccountId, "IX_Order_account_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");
                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(10)
                    .HasDefaultValue("cancelled")
                    .HasColumnName("payment_status");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");
                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total_amount");

                entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Order__account_i__5535A963");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__3C5A4080CB8FC91C");

                entity.ToTable("OrderDetail");

                entity.HasIndex(e => e.OrderId, "IX_OrderDetail_order_id");

                entity.HasIndex(e => e.ProductdetailId, "IX_OrderDetail_productdetail_id");

                entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.ProductdetailId).HasColumnName("productdetail_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__order__5812160E");

                entity.HasOne(d => d.Productdetail).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductdetailId)
                    .HasConstraintName("FK__OrderDeta__produ__59063A47");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(e => e.PhotoId).HasName("PK__Photo__CB48C83D51872FDC");

                entity.ToTable("Photo");

                entity.HasIndex(e => e.ProductId, "IX_Photo_product_id");

                entity.Property(e => e.PhotoId).HasColumnName("photo_id");
                entity.Property(e => e.PhotoName)
                    .HasMaxLength(100)
                    .HasColumnName("photo_name");
                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(100)
                    .HasColumnName("photo_url");
                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Product).WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Photo__product_i__47DBAE45");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF5F3E00BD2");

                entity.ToTable("Product");

                entity.HasIndex(e => e.BrandId, "IX_Product_brand_id");

                entity.HasIndex(e => e.CategoryId, "IX_Product_category_id");

                entity.HasIndex(e => e.SupplierId, "IX_Product_supplier_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.BrandId).HasColumnName("brand_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_name");
                entity.Property(e => e.PurchasePrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("purchase_price");
                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");

                entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Product__brand_i__4316F928");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__categor__440B1D61");

                entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Product__supplie__44FF419A");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductdetailId).HasName("PK__ProductD__A823340D55986CEB");

                entity.ToTable("ProductDetail");

                entity.HasIndex(e => e.ColorId, "IX_ProductDetail_color_id");

                entity.HasIndex(e => e.ProductId, "IX_ProductDetail_product_id");

                entity.HasIndex(e => e.SizeId, "IX_ProductDetail_size_id");

                entity.Property(e => e.ProductdetailId).HasColumnName("productdetail_id");
                entity.Property(e => e.ColorId).HasColumnName("color_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.SellingPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("selling_price");
                entity.Property(e => e.SizeId).HasColumnName("size_id");

                entity.HasOne(d => d.Color).WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK__ProductDe__color__4CA06362");

                entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__ProductDe__produ__4AB81AF0");

                entity.HasOne(d => d.Size).WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("FK__ProductDe__size___4BAC3F29");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.HasKey(e => e.SizeId).HasName("PK__Size__0DCACE3135AEF154");

                entity.ToTable("Size");

                entity.Property(e => e.SizeId).HasColumnName("size_id");
                entity.Property(e => e.SizeCode).HasDefaultValue("");
                entity.Property(e => e.SizeName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("size_name");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__6EE594E8AE47FBB9");

                entity.ToTable("Supplier");

                entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");
                entity.Property(e => e.SupplierName)
                    .HasMaxLength(100)
                    .HasColumnName("supplier_name");
            });
        }
        /*private void SeedRoles(ModelBuilder builder) {
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
        }*/
    }
}

using Microsoft.EntityFrameworkCore;

namespace WebShop.Models
{
    public partial class WebShopContext : DbContext
    {
        public WebShopContext()
        {
        }

        public WebShopContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductProductCategory> ProductProductCategory { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<ShoppingCartProducts> ShoppingCartProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=WebShop;Trusted_connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.CustomerId).HasColumnName("customer_Id");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShoppingCartId).HasColumnName("shopping_cart_Id");

                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ShoppingCartId)
                    .HasConstraintName("FK_customer_shopping_cart");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.OrderId).HasColumnName("order_Id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_Id");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("order_date")
                    .HasColumnType("date");

                entity.Property(e => e.ShipAddress)
                    .IsRequired()
                    .HasColumnName("ship_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipDate)
                    .HasColumnName("ship_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_order_customer");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });

                entity.ToTable("order_detail");

                entity.Property(e => e.OrderId).HasColumnName("order_Id");

                entity.Property(e => e.ProductId).HasColumnName("product_Id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_detail_order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_detail_product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.ProductId).HasColumnName("product_Id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("product_category");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_Id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.ProductCategoryId, e.ProductId });

                entity.ToTable("product_product_category");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_Id");

                entity.Property(e => e.ProductId).HasColumnName("product_Id");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.ProductProductCategory)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_product_category_product_category");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductProductCategory)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_product_product_category_product");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("shopping_cart");

                entity.Property(e => e.ShoppingCartId).HasColumnName("shopping_cart_Id");
            });

            modelBuilder.Entity<ShoppingCartProducts>(entity =>
            {
                entity.HasKey(e => new { e.ShoppingCartId, e.ProductId });

                entity.ToTable("shopping_cart_products");

                entity.Property(e => e.ShoppingCartId).HasColumnName("shopping_cart_Id");

                entity.Property(e => e.ProductId).HasColumnName("product_Id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCartProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shopping_cart_products_product");

                entity.HasOne(d => d.ShoppingCart)
                    .WithMany(p => p.ShoppingCartProducts)
                    .HasForeignKey(d => d.ShoppingCartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_shopping_cart_products_shopping_cart");
            });
        }
    }
}

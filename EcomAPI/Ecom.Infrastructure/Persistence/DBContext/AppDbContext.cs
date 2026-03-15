using Ecom.Domain.Entities;
using Ecom.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<SpecificationKey> SpecificationKeys { get; set; }
        public DbSet<CategorySpecificationKey> CategorySpecificationKeys { get; set; }
        public DbSet<ProductSpecificationKey> ProductSpecificationKeys { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. User - Address: One-to-Many
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_addresses");

            // 2. User - RefreshToken: One-to-Many
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_refresh_tokens");

            // 3. User - Cart: One-to-One
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_carts");

            // 4. Cart - CartItem: One-to-Many
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_carts_cart_items");

            // 5. Order - OrderItem: One-to-Many
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_orders_order_items");

            // 6. Order - Payment: One-to-One
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne()
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_orders_payments");

            // 7. Product - Category/Brand: Many-to-One
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_categories_products");
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_brands_products");

            // 8. Product - ProductImage: One-to-Many
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_products_product_images");

            // 9. SpecificationKeys - Category/Product: Many-to-Many
            modelBuilder.Entity<CategorySpecificationKey>()
                .HasOne(csk => csk.Category)
                .WithMany(c => c.CategorySpecificationKeys)
                .HasForeignKey(csk => csk.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_categories_category_spec_keys");
            modelBuilder.Entity<CategorySpecificationKey>()
                .HasOne(csk => csk.SpecificationKey)
                .WithMany(sk => sk.CategorySpecificationKeys)
                .HasForeignKey(csk => csk.SpecificationKeyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_spec_keys_category_spec_keys");
            modelBuilder.Entity<ProductSpecificationKey>()
                .HasOne(psk => psk.Product)
                .WithMany(p => p.ProductSpecificationKeys)
                .HasForeignKey(psk => psk.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_products_product_spec_keys");
            modelBuilder.Entity<ProductSpecificationKey>()
                .HasOne(psk => psk.SpecificationKey)
                .WithMany(sk => sk.ProductSpecificationKeys)
                .HasForeignKey(psk => psk.SpecificationKeyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_spec_keys_product_spec_keys");
            // 10. User - EmailVerificationToken: One-to-Many
            modelBuilder.Entity<EmailVerificationToken>()
                .HasOne(evt => evt.User)
                .WithMany(u => u.EmailVerificationTokens)
                .HasForeignKey(evt => evt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_email_verification_tokens");

            // ID properties configuration
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<RefreshToken>().HasKey(p => p.Id);
            modelBuilder.Entity<Cart>().HasKey(p => p.Id);
            modelBuilder.Entity<Order>().HasKey(p => p.Id);
            modelBuilder.Entity<Payment>().HasKey(p => p.Id);
            modelBuilder.Entity<EmailVerificationToken>().HasKey(p => p.Id);

            // int IDs auto-increment
            modelBuilder.Entity<CartItem>().HasKey(ci => ci.Id);
            modelBuilder.Entity<CartItem>().Property(ci => ci.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductImage>().HasKey(pi => pi.Id);
            modelBuilder.Entity<ProductImage>().Property(pi => pi.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Brand>().HasKey(b => b.Id);
            modelBuilder.Entity<Brand>().Property(b => b.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SpecificationKey>().HasKey(sk => sk.Id);
            modelBuilder.Entity<SpecificationKey>().Property(sk => sk.Id).ValueGeneratedOnAdd();

            // Composite keys for many-to-many join tables
            modelBuilder.Entity<CategorySpecificationKey>().HasKey(csk => new { csk.CategoryId, csk.SpecificationKeyId });
            modelBuilder.Entity<ProductSpecificationKey>().HasKey(psk => new { psk.ProductId, psk.SpecificationKeyId });

            // Property configurations + Required + MaxLength
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(500);
            });
            modelBuilder.Entity<Address>(b =>
            {
                b.Property(a => a.AddressText).IsRequired().HasMaxLength(500);
            });
            modelBuilder.Entity<Cart>(b =>
            {
                b.Property(c => c.SessionID).HasMaxLength(255);
            });
            modelBuilder.Entity<Product>(b =>
            {
                b.Property(p => p.Name).IsRequired().HasMaxLength(255);
                b.Property(p => p.Description).HasMaxLength(2000);
                b.Property(p => p.Price).HasPrecision(18, 3); // Decimal precision

                b.Property(p => p.Slug).IsRequired().HasMaxLength(255);
                b.HasIndex(p => p.Slug).IsUnique().HasDatabaseName("IX_Products_Slug_Unique");
            });
            modelBuilder.Entity<Category>(b =>
            {
                b.Property(c => c.Name).IsRequired().HasMaxLength(255);
                b.Property(c => c.Slug).IsRequired().HasMaxLength(255);
                b.HasIndex(c => c.Slug).IsUnique().HasDatabaseName("IX_Categories_Slug_Unique");
            });
            modelBuilder.Entity<Brand>(b =>
            {
                b.Property(br => br.Name).IsRequired().HasMaxLength(255);
                b.Property(br => br.Slug).IsRequired().HasMaxLength(255);

                b.HasIndex(br => br.Slug)
                    .IsUnique()
                    .HasDatabaseName("IX_Brands_Slug_Unique");
            });
            modelBuilder.Entity<Order>(b =>
            {
                b.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
                b.Property(o => o.Status).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<Payment>(b =>
            {
                b.Property(p => p.TransactionCode).HasMaxLength(200);
                b.Property(p => p.Provider).HasMaxLength(20);
            });
            modelBuilder.Entity<SpecificationKey>(b =>
            {
                b.Property(sk => sk.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property(sk => sk.Unit)
                    .HasMaxLength(50);
            });
            modelBuilder.Entity<CategorySpecificationKey>(b =>
            {
                b.Property(x => x.DisplayOrder)
                    .IsRequired();
            });
            modelBuilder.Entity<ProductSpecificationKey>(b =>
            {
                b.Property(psk => psk.Value)
                    .IsRequired()
                    .HasMaxLength(500);
            });
            modelBuilder.Entity<EmailVerificationToken>(b =>
            {
                b.Property(evt => evt.Token)
                    .IsRequired();
                b.HasIndex(evt => evt.Token)
                   .IsUnique();
                b.Property(evt => evt.ExpiresAt)
                    .IsRequired();
                b.Property(evt => evt.IsUsed)
                    .IsRequired();
                b.Property(evt => evt.UserId)
                    .IsRequired();
            });

            // Unique Indexes
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique");

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token_Unique");

            // Performance Indexes
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UserId)
                .HasDatabaseName("IX_Orders_UserId");

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Status)
                .HasDatabaseName("IX_Orders_Status");

            modelBuilder.Entity<Cart>()
                .HasIndex(c => c.SessionID)
                .HasDatabaseName("IX_Carts_SessionID");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Products_CategoryId");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.BrandId)
                .HasDatabaseName("IX_Products_BrandId");

            // SeedData
            SeedData(modelBuilder);

        }

        private static readonly Guid AdminId = new("550e8400-e29b-41d4-a716-446655440000");
        private static void SeedData(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(new
            {
                Id = AdminId,
                Name = "Admin",
                Email = "admin@ecom.com",
                PhoneNumber = "0839095701",
                Dob = new DateOnly(2003, 01, 31),
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsActive = true,
                Role = UserRole.Admin,
                CartID = AdminId,
                IsDeleted = false,
            });
        }
    }
}

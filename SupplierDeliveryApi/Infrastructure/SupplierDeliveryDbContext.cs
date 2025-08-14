using Microsoft.EntityFrameworkCore;
using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Infrastructure
{
    public class SupplierDeliveryDbContext : DbContext
    {
        public SupplierDeliveryDbContext(DbContextOptions<SupplierDeliveryDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryProduct> DeliveryProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supplier>().ToTable("Suppliers");
            modelBuilder.Entity<Supplier>().HasKey(s => s.IdSupplier);
            modelBuilder.Entity<Supplier>().Property(s => s.IdSupplier).ValueGeneratedOnAdd();
            modelBuilder.Entity<Supplier>().Property(s => s.NmSupplier).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().HasKey(p => p.IdProduct);
            modelBuilder.Entity<Product>().Property(p => p.IdProduct).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(p => p.NmProduct).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired().HasColumnType("money");

            modelBuilder.Entity<Delivery>().ToTable("Deliveries");
            modelBuilder.Entity<Delivery>().HasKey(d => d.IdDelivery);
            modelBuilder.Entity<Delivery>().Property(d => d.IdDelivery).ValueGeneratedOnAdd();
            modelBuilder.Entity<Delivery>().Property(d => d.DtDelivery).IsRequired().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Delivery>().Property(d => d.DsStatus).IsRequired().HasConversion<string>();
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Supplier)
                .WithMany(s => s.Deliveries)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliveryAddress>().ToTable("DeliveryAddresses");
            modelBuilder.Entity<DeliveryAddress>().HasKey(da => da.IdDelivery);
            modelBuilder.Entity<DeliveryAddress>()
                .HasOne(da => da.Delivery)
                .WithOne(d => d.Address)
                .HasForeignKey<DeliveryAddress>(da => da.IdDelivery);

            modelBuilder.Entity<DeliveryProduct>().ToTable("DeliveryProducts");
            modelBuilder.Entity<DeliveryProduct>().HasKey(dp => new { dp.IdDelivery, dp.IdProduct }); // Chave composta
            modelBuilder.Entity<DeliveryProduct>()
                .HasOne(dp => dp.Delivery)
                .WithMany(d => d.Products)
                .HasForeignKey(dp => dp.IdDelivery);

            modelBuilder.Entity<DeliveryProduct>().Property(dp => dp.QtProduct).IsRequired();
        }
    }
}
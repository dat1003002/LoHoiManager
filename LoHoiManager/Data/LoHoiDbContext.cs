using Microsoft.EntityFrameworkCore;
using LoHoiManager.model;

namespace LoHoiManager.Data
{
    public class LoHoiDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Pallet> Pallets { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<ExportProduct> ExportProducts { get; set; }

        public LoHoiDbContext() { }

        public LoHoiDbContext(DbContextOptions<LoHoiDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Configuration.Instance.GetConnectionString();
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ cho Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Pallet)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.PalletId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Factory)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FactoryId);

            // Quan hệ cho ExportProduct
            modelBuilder.Entity<ExportProduct>()
                .HasOne(ep => ep.Pallet)
                .WithMany(p => p.ExportProducts)
                .HasForeignKey(ep => ep.PalletId);

            modelBuilder.Entity<ExportProduct>()
                .HasOne(ep => ep.Supplier)
                .WithMany(s => s.ExportProducts)
                .HasForeignKey(ep => ep.SupplierId);

            modelBuilder.Entity<ExportProduct>()
                .HasOne(ep => ep.Factory)
                .WithMany(f => f.ExportProducts)
                .HasForeignKey(ep => ep.FactoryId);

            // Ánh xạ ProductStatus thành string
            modelBuilder.Entity<Product>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ExportProduct>()
                .Property(ep => ep.Status)
                .HasConversion<string>();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Sciensoft.Samples.Products.Api.Infrastructure.Models;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Repository
{
    public class ProductsDbContext : DbContext
    {
        readonly string _connectionString;

        public ProductsDbContext()
            : base ()
        { }

        public ProductsDbContext(DbContextOptions options)
            : base(options)
        { }

        public ProductsDbContext(string connectionString)
            => _connectionString = connectionString;

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            void ProductConfiguration()
            {
                var entity = modelBuilder.Entity<Product>();

                entity.HasKey(p => p.Id)
                    .HasName("PK_ProductId");

                entity.HasIndex(p => p.Name);

                entity.HasIndex(p => p.Code)
                    .IsUnique();
            }

            ProductConfiguration();
        }
    }
}

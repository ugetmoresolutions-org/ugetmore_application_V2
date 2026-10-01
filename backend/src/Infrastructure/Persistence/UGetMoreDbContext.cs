using Microsoft.EntityFrameworkCore;
using UGetMore.Domain.Entities;

namespace UGetMore.Infrastructure.Persistence;

public class UGetMoreDbContext : DbContext
{
    public UGetMoreDbContext(DbContextOptions<UGetMoreDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<SupplierSku> SupplierSkus => Set<SupplierSku>();
    public DbSet<Stock> Stock => Set<Stock>();
    public DbSet<BrandingOption> BrandingOptions => Set<BrandingOption>();
    public DbSet<BrandingPriceTier> BrandingPriceTiers => Set<BrandingPriceTier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UGetMoreDbContext).Assembly);
    }
}

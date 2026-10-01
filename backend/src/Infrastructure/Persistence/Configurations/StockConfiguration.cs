using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGetMore.Domain.Entities;

namespace UGetMore.Infrastructure.Persistence.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stock");
        builder.HasKey(s => s.Id);

        // QuantityOnHand/QuantityReserved only change through Stock's guarded methods (Reserve/
        // Release/Commit) — EF Core materializes via the private setters, callers cannot bypass them.
        builder.Property(s => s.QuantityOnHand);
        builder.Property(s => s.QuantityReserved);
        builder.Ignore(s => s.QuantityAvailable);

        builder.HasOne(s => s.ProductVariant)
            .WithOne(v => v.Stock)
            .HasForeignKey<Stock>(s => s.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ProductVariantId).IsUnique();
    }
}

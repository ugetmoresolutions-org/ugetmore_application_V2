using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGetMore.Domain.Entities;

namespace UGetMore.Infrastructure.Persistence.Configurations;

public class SupplierSkuConfiguration : IEntityTypeConfiguration<SupplierSku>
{
    public void Configure(EntityTypeBuilder<SupplierSku> builder)
    {
        builder.ToTable("SupplierSkus");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Supplier).HasConversion<string>().HasMaxLength(50);
        builder.Property(s => s.SupplierProductCode).IsRequired().HasMaxLength(200);
        builder.Property(s => s.LastSyncedPrice).HasColumnType("numeric(18,2)");

        // A supplier's own code must map to at most one variant.
        builder.HasIndex(s => new { s.Supplier, s.SupplierProductCode }).IsUnique();

        builder.HasOne(s => s.ProductVariant)
            .WithMany(v => v.SupplierSkus)
            .HasForeignKey(s => s.ProductVariantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

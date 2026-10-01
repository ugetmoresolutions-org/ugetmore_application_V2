using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGetMore.Domain.Entities;

namespace UGetMore.Infrastructure.Persistence.Configurations;

public class BrandingOptionConfiguration : IEntityTypeConfiguration<BrandingOption>
{
    public void Configure(EntityTypeBuilder<BrandingOption> builder)
    {
        builder.ToTable("BrandingOptions");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.BrandingCode).IsRequired().HasMaxLength(100);
        builder.Property(b => b.BrandingMethod).IsRequired().HasMaxLength(100);
        builder.Property(b => b.DesignFee).HasColumnType("numeric(18,2)");

        builder.HasOne(b => b.ProductVariant)
            .WithMany()
            .HasForeignKey(b => b.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BrandingPriceTierConfiguration : IEntityTypeConfiguration<BrandingPriceTier>
{
    public void Configure(EntityTypeBuilder<BrandingPriceTier> builder)
    {
        builder.ToTable("BrandingPriceTiers");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.SetupFee).HasColumnType("numeric(18,2)");
        builder.Property(t => t.UnitPrice).HasColumnType("numeric(18,2)");

        builder.HasOne(t => t.BrandingOption)
            .WithMany(b => b.PriceTiers)
            .HasForeignKey(t => t.BrandingOptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

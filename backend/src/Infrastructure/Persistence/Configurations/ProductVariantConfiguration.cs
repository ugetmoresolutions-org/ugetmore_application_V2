using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGetMore.Domain.Entities;

namespace UGetMore.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.InternalSku).IsRequired().HasMaxLength(100);
        builder.Property(v => v.BasePrice).HasColumnType("numeric(18,2)");
        builder.HasIndex(v => v.InternalSku).IsUnique();

        builder.HasOne(v => v.Product)
            .WithMany(p => p.Variants)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

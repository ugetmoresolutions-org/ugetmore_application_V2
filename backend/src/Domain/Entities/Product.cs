using UGetMore.Domain.Enums;

namespace UGetMore.Domain.Entities;

// The one canonical Product aggregate, replacing the 7-8 overlapping "Product" interfaces found
// across the frontend (newProduct.IProduct, product.IProduct, unified-product.IUnifiedProduct,
// aggregated-product.IAggregatedProduct, brandingProduct.IBrandingProduct, furniture.IFurnitureProduct,
// and coupon.ts's own locally-redeclared IProduct). ProductType discriminates the verticals that used
// to be separate near-duplicate interfaces instead of adding a parallel table per vertical.
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductType ProductType { get; set; }
    public ProductStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}

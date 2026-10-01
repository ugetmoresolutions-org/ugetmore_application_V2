namespace UGetMore.Domain.Entities;

// Size/colour/sku-level variant of a Product. This is the entity SupplierSku maps onto, and the
// entity Stock, BrandingPriceTier and CartItem/OrderLine all key off — one variant identity shared
// across every supplier source instead of each supplier's own code being treated as the product id.
public class ProductVariant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string InternalSku { get; set; } = string.Empty;
    public string? ColorCode { get; set; }
    public string? ColorName { get; set; }
    public string? SizeCode { get; set; }
    public string? SizeName { get; set; }
    public decimal BasePrice { get; set; }
    public int MinimumQuantity { get; set; } = 1;
    public int? MaximumQuantity { get; set; }

    public Product Product { get; set; } = null!;
    public ICollection<SupplierSku> SupplierSkus { get; set; } = new List<SupplierSku>();
    public Stock? Stock { get; set; }
}

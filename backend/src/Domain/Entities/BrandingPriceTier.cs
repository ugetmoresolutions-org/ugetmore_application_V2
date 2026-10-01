namespace UGetMore.Domain.Entities;

// Quantity-tiered pricing for a BrandingOption. Replaces IBrandingPrice's client-trusted tier list —
// the server picks the tier covering the requested quantity instead of falling back to a hardcoded
// flat rate when no tier matches.
public class BrandingPriceTier
{
    public Guid Id { get; set; }
    public Guid BrandingOptionId { get; set; }
    public int MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }
    public decimal SetupFee { get; set; }
    public decimal UnitPrice { get; set; }

    public BrandingOption BrandingOption { get; set; } = null!;
}

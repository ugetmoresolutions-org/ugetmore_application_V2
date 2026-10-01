namespace UGetMore.Domain.Entities;

// Canonical home for branding/decoration pricing, replacing the client-side R225-setup/R12-per-item
// and R250-per-position fallback constants in components/shop/branding/brandingPricing.ts. A null
// ProductVariantId means the branding method applies globally (not tied to one variant).
public class BrandingOption
{
    public Guid Id { get; set; }
    public Guid? ProductVariantId { get; set; }
    public string BrandingCode { get; set; } = string.Empty;
    public string BrandingMethod { get; set; } = string.Empty;
    public decimal DesignFee { get; set; }

    public ProductVariant? ProductVariant { get; set; }
    public ICollection<BrandingPriceTier> PriceTiers { get; set; } = new List<BrandingPriceTier>();
}

using UGetMore.Domain.Enums;

namespace UGetMore.Domain.Entities;

// Maps one supplier's own product identity (Amrod simpleCode/fullCode, Parrot stock code, Tarsus
// product code, or an internally-created SKU) onto a single internal ProductVariant. This is the
// reconciliation point that lets aggregated-product search and grade-stationery code resolution
// work against one product graph instead of three independent supplier-shaped payloads.
public class SupplierSku
{
    public Guid Id { get; set; }
    public Guid? ProductVariantId { get; set; }
    public SupplierCode Supplier { get; set; }
    public string SupplierProductCode { get; set; } = string.Empty;
    public decimal? LastSyncedPrice { get; set; }
    public int? LastSyncedStock { get; set; }
    public string? RawPayloadHash { get; set; }
    public DateTimeOffset? LastSyncedAt { get; set; }

    public ProductVariant? ProductVariant { get; set; }
}

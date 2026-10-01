namespace UGetMore.Domain.Entities;

// One stock row per ProductVariant, replacing the three redundant keys (by id, by productId, by
// fullCode) the frontend's stock.ts wrapper exposes today. Reservation is an explicit, guarded
// operation rather than a quantity the client can set directly.
public class Stock
{
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public int QuantityOnHand { get; private set; }
    public int QuantityReserved { get; private set; }
    public int ReorderLevel { get; set; }

    public int QuantityAvailable => QuantityOnHand - QuantityReserved;

    public ProductVariant ProductVariant { get; set; } = null!;

    // EF Core needs a parameterless constructor; keep it protected so callers go through the
    // guarded factory/mutation methods below instead of setting fields directly.
    protected Stock() { }

    public Stock(Guid productVariantId, int quantityOnHand)
    {
        if (quantityOnHand < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantityOnHand), "Quantity on hand cannot be negative.");
        }

        ProductVariantId = productVariantId;
        QuantityOnHand = quantityOnHand;
    }

    // Reserves stock for a pending order line. Throws rather than allowing QuantityAvailable to go
    // negative — the one place "can this be purchased" is decided, instead of the frontend's current
    // fallback of treating missing/unknown stock as 999-available.
    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Reservation quantity must be positive.");
        }

        if (quantity > QuantityAvailable)
        {
            throw new InvalidOperationException(
                $"Cannot reserve {quantity} units; only {QuantityAvailable} available.");
        }

        QuantityReserved += quantity;
    }

    public void Release(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Release quantity must be positive.");
        }

        if (quantity > QuantityReserved)
        {
            throw new InvalidOperationException(
                $"Cannot release {quantity} units; only {QuantityReserved} are reserved.");
        }

        QuantityReserved -= quantity;
    }

    // Commits a reservation at payment confirmation: reserved stock is removed from on-hand as well,
    // since it has now actually left the warehouse (or been allocated), rather than just held.
    public void Commit(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Commit quantity must be positive.");
        }

        if (quantity > QuantityReserved)
        {
            throw new InvalidOperationException(
                $"Cannot commit {quantity} units; only {QuantityReserved} are reserved.");
        }

        QuantityReserved -= quantity;
        QuantityOnHand -= quantity;
    }
}

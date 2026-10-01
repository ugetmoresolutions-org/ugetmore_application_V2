namespace UGetMore.Domain.Enums;

// The four product sources the frontend currently reconciles ad hoc (Amrod/Parrot/Tarsus feeds
// plus internally-created products). SupplierSku maps each one's own code to a single ProductVariant.
public enum SupplierCode
{
    Internal,
    Amrod,
    Parrot,
    Tarsus,
}

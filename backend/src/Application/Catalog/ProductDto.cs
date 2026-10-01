namespace UGetMore.Application.Catalog;

public record ProductVariantDto(
    Guid Id,
    string InternalSku,
    string? ColorCode,
    string? ColorName,
    string? SizeCode,
    string? SizeName,
    decimal BasePrice,
    int MinimumQuantity,
    int? MaximumQuantity,
    int? QuantityAvailable
);

public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    string ProductType,
    string Status,
    Guid CategoryId,
    IReadOnlyList<ProductVariantDto> Variants
);

public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);

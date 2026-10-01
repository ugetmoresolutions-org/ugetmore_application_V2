namespace UGetMore.Application.Catalog;

public record CategoryDto(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    Guid? ParentCategoryId,
    int DisplayOrder
);

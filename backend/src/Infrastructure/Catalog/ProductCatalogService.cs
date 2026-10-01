using Microsoft.EntityFrameworkCore;
using UGetMore.Application.Catalog;
using UGetMore.Infrastructure.Persistence;

namespace UGetMore.Infrastructure.Catalog;

public class ProductCatalogService : IProductCatalogService
{
    private readonly UGetMoreDbContext _db;

    public ProductCatalogService(UGetMoreDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _db.Categories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new CategoryDto(c.Id, c.Name, c.Code, c.Description, c.ParentCategoryId, c.DisplayOrder))
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;

        var query = _db.Products.AsNoTracking().OrderBy(p => p.Name);

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ProjectToDto())
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductDto>(products, page, pageSize, totalCount);
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ProjectToDto())
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static System.Linq.Expressions.Expression<Func<Domain.Entities.Product, ProductDto>> ProjectToDto() =>
        p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.ProductType.ToString(),
            p.Status.ToString(),
            p.CategoryId,
            p.Variants.Select(v => new ProductVariantDto(
                v.Id,
                v.InternalSku,
                v.ColorCode,
                v.ColorName,
                v.SizeCode,
                v.SizeName,
                v.BasePrice,
                v.MinimumQuantity,
                v.MaximumQuantity,
                // Computed via the raw mapped columns rather than the Stock.QuantityAvailable
                // property, since EF Core's SQL translator can't always inline a computed getter.
                v.Stock != null ? v.Stock.QuantityOnHand - v.Stock.QuantityReserved : (int?)null
            )).ToList()
        );
}

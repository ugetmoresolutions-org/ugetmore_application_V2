namespace UGetMore.Application.Catalog;

// The Phase 1 read surface: GET /categories, GET /products, GET /products/{id}, consumed by the
// frontend's aggregated-product search facade (endpoints/rest-api/aggregated-product.ts), which
// already targets the business baseUrl rather than a supplier proxy.
public interface IProductCatalogService
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken);

    Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}

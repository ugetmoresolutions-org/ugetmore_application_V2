using Microsoft.AspNetCore.Mvc;
using UGetMore.Application.Catalog;
using UGetMore.Application.Common;

namespace UGetMore.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductCatalogService _catalog;

    public ProductsController(IProductCatalogService catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public async Task<ActionResult<CustomResponse<PagedResult<ProductDto>>>> GetProducts(
        [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var products = await _catalog.GetProductsAsync(page, pageSize, cancellationToken);
        return Ok(CustomResponse<PagedResult<ProductDto>>.Ok(products));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomResponse<ProductDto>>> GetProductById(
        Guid id, CancellationToken cancellationToken)
    {
        var product = await _catalog.GetProductByIdAsync(id, cancellationToken);

        // 404 + ProblemDetails for "not found" — distinct from a service outage, which the exception
        // handler maps to 5xx. The frontend's current server-product-loader.ts collapses both into
        // null -> not-found; the new contract lets it tell them apart.
        if (product is null)
        {
            return Problem(
                title: "Product not found",
                statusCode: StatusCodes.Status404NotFound,
                detail: $"No product exists with id '{id}'.");
        }

        return Ok(CustomResponse<ProductDto>.Ok(product));
    }
}

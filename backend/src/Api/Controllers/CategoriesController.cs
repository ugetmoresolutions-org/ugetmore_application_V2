using Microsoft.AspNetCore.Mvc;
using UGetMore.Application.Catalog;
using UGetMore.Application.Common;

namespace UGetMore.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IProductCatalogService _catalog;

    public CategoriesController(IProductCatalogService catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public async Task<ActionResult<CustomResponse<IReadOnlyList<CategoryDto>>>> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await _catalog.GetCategoriesAsync(cancellationToken);
        return Ok(CustomResponse<IReadOnlyList<CategoryDto>>.Ok(categories));
    }
}

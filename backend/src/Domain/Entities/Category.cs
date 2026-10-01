namespace UGetMore.Domain.Entities;

// Single canonical category tree, replacing the three conflicting ICategory shapes found in the
// frontend (two duplicate declarations in interfaces/product/category.ts, a third ad hoc shape in
// endpoints/rest-api/categories.ts).
public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public int DisplayOrder { get; set; }

    public Category? ParentCategory { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Factories;

namespace Granitos.Services.Infrastructure.Factories.ProductCategories;

public class CreateProductCategoryFactory : ICreateEntityFactory<ProductCategory>
{
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly string _name;
    private readonly IReadOnlyList<string> _tags;

    public CreateProductCategoryFactory(IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags,
        string name)
    {
        _metadata = metadata;
        _tags = tags;
        _name = name;
    }

    public ProductCategory Create()
    {
        return new ProductCategory(
            Guid.NewGuid(),
            DateTime.UtcNow,
            string.Empty,
            DateTime.UtcNow,
            string.Empty,
            default,
            string.Empty,
            _metadata,
            _tags,
            _name);
    }
}
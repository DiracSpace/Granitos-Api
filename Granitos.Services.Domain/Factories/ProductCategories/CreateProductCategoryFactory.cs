using Granitos.Services.Domain.Entities;

namespace Granitos.Services.Domain.Factories.ProductCategories;

public class CreateProductCategoryFactory : ICreateEntityFactory<ProductCategory>
{
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly IReadOnlyList<string> _tags;
    private readonly string _name;

    public CreateProductCategoryFactory(IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags,
        string name)
    {
        _metadata = metadata;
        _tags = tags;
        _name = name;
    }

    public ProductCategory Create()
        => new(
            Guid.NewGuid(),
            createdAt: DateTime.UtcNow,
            createdBy: string.Empty,
            updatedAt: DateTime.UtcNow,
            updatedBy: string.Empty,
            deletedAt: default,
            deletedBy: string.Empty,
            _metadata,
            _tags,
            _name);
}
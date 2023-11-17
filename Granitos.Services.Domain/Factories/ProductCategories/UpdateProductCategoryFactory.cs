using Granitos.Services.Domain.Entities;

namespace Granitos.Services.Domain.Factories.ProductCategories;

public sealed class UpdateProductCategoryFactory : IUpdateEntityFactory<ProductCategory> 
{
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly IReadOnlyList<string> _tags;
    private readonly string _name;

    public UpdateProductCategoryFactory(IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags, string name)
    {
        _metadata = metadata;
        _tags = tags;
        _name = name;
    }

    public void Update(ProductCategory entity)
    {
        entity.SetName(_name);
        entity.SetMetadata(_metadata);
        entity.SetTags(_tags);
    }
}
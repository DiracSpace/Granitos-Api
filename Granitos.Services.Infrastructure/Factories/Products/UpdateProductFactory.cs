using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Factories;

namespace Granitos.Services.Infrastructure.Factories.Products;

public class UpdateProductFactory : IUpdateEntityFactory<Product>
{
    private readonly string _code;
    private readonly string _description;
    private readonly string _imageUrl;
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly string _name;
    private readonly ProductCategory _productCategory;
    private readonly IReadOnlyList<string> _tags;
    private readonly int _unitInStock;
    private readonly decimal _unitPrice;

    public UpdateProductFactory(string name, string code, string description, string imageUrl, decimal unitPrice,
        int unitInStock, ProductCategory productCategory, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags)
    {
        _name = name;
        _code = code;
        _description = description;
        _imageUrl = imageUrl;
        _unitPrice = unitPrice;
        _unitInStock = unitInStock;
        _productCategory = productCategory;
        _metadata = metadata;
        _tags = tags;
    }

    public void Update(Product entity)
    {
        entity.SetName(_name);
        entity.SetCode(_code);
        entity.SetDescription(_description);
        entity.SetImageUrl(_imageUrl);
        entity.SetUnitPrice(_unitPrice);
        entity.SetUnitInStock(_unitInStock);
        entity.SetProductCategory(_productCategory);
        entity.SetMetadata(_metadata);
        entity.SetTags(_tags);
    }
}
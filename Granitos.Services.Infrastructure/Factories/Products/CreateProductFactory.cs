using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Factories;

namespace Granitos.Services.Infrastructure.Factories.Products;

public class CreateProductFactory : ICreateEntityFactory<Product>
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

    public CreateProductFactory(string name, string code, string description, string imageUrl, decimal unitPrice,
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

    public Product Create()
    {
        return new Product(
            Guid.NewGuid(),
            DateTime.UtcNow,
            string.Empty,
            DateTime.UtcNow,
            string.Empty,
            default,
            string.Empty,
            _metadata,
            _tags,
            _name,
            _code,
            _description,
            _imageUrl,
            _unitPrice,
            _unitInStock,
            _productCategory);
    }
}
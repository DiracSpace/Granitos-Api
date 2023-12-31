namespace Granitos.Services.Domain.Entities;

public sealed class Product : Entity
{
    public Product(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags, string name, string code, string description, string imageUrl, decimal unitPrice,
        int unitInStock, ProductCategory category) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt,
        deletedBy, metadata, tags)
    {
        Name = name;
        Code = code;
        Description = description;
        ImageUrl = imageUrl;
        UnitPrice = unitPrice;
        UnitInStock = unitInStock;
        Category = category;
    }

    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int UnitInStock { get; private set; }
    public ProductCategory Category { get; private set; }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetCode(string code)
    {
        Code = code;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public void SetUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
    }

    public void SetUnitInStock(int unitInStock)
    {
        UnitInStock = unitInStock;
    }

    public void SetProductCategory(ProductCategory category)
    {
        Category = category;
    }
}
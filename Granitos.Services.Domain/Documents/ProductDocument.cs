namespace Granitos.Services.Domain.Documents;

public sealed class ProductDocument : MongoDocument
{
    public const string CollectionName = "products";

    public ProductDocument(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, Dictionary<string, string> metadata, List<string>? tags, string name,
        string code, string description, string imageUrl, decimal unitPrice, int unitInStock,
        ProductCategoryDocument category) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy,
        metadata, tags)
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
    public ProductCategoryDocument Category { get; private set; }
}
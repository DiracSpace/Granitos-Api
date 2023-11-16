namespace Granitos.Services.Domain.Documents;

public sealed class ProductCategoryDocument : MongoDocument
{
    public const string CollectionName = "product-categories";

    public ProductCategoryDocument(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt,
        string? updatedBy, DateTime? deletedAt, string? deletedBy, Dictionary<string, string> metadata,
        List<string>? tags, string name) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy,
        metadata, tags)
    {
        Name = name;
    }

    public string Name { get; private set; }
}
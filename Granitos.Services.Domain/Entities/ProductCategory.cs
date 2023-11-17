namespace Granitos.Services.Domain.Entities;

public sealed class ProductCategory : Entity
{
    public ProductCategory(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags, string name) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt,
        deletedBy, metadata, tags)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public void SetName(string name)
    {
        Name = name;
    }
}
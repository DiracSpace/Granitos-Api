namespace Granitos.Services.Domain.Entities;

public sealed class ProductCategory : Entity
{
    public ProductCategory(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, DateTime? deletedAt,
        IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags, string name) : base(id, createdAt,
        createdBy, updatedAt, deletedAt, metadata, tags)
    {
        Name = name;
    }

    public string Name { get; private set; }
}
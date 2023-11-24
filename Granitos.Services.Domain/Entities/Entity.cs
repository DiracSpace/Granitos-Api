namespace Granitos.Services.Domain.Entities;

public abstract class Entity
{
    private readonly Dictionary<string, string> _metadata = new();
    private readonly List<string> _tags = new();

    protected Entity(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;

        SetMetadata(metadata);
        SetTags(tags);
    }

    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public DateTime? UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public DateTime? DeletedAt { get; }
    public string? DeletedBy { get; }

    public IReadOnlyDictionary<string, string> Metadata => _metadata;

    public IReadOnlyList<string> Tags => _tags;

    public void SetMetadata(IReadOnlyDictionary<string, string> metadata)
    {
        _metadata.Clear();

        foreach (var value in metadata)
            _metadata.Add(value.Key, value.Value);
    }

    public void SetTags(IReadOnlyList<string> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags);
    }
}
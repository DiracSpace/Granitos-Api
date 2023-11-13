namespace Granitos.Services.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }

    public DateTime? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; } = string.Empty;

    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; } = string.Empty;

    public IReadOnlyDictionary<string, string> Metadata => _metadata;
    private readonly Dictionary<string, string> _metadata = new();

    public IReadOnlyList<string> Tags => _tags;
    private readonly List<string> _tags = new();

    protected Entity(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, DateTime? deletedAt,
        IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;

        SetMetadata(metadata);
        SetTags(tags);
    }

    protected void SetMetadata(IReadOnlyDictionary<string, string> metadata)
    {
        _metadata.Clear();

        foreach (var value in metadata)
            _metadata.Add(value.Key, value.Value);
    }

    protected void SetTags(IReadOnlyList<string> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags);
    }
}
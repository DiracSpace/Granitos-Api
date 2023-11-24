using Granitos.Common.Mongo.Repositories.Abstractions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Granitos.Services.Domain.Documents;

public class MongoDocument : IMongoDocument
{
    protected MongoDocument(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, Dictionary<string, string> metadata, List<string>? tags)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
        Metadata = metadata;
        Tags = tags;
    }

    public Dictionary<string, string> Metadata { get; set; }

    [BsonRepresentation(BsonType.String)] public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public List<string>? Tags { get; set; }
}
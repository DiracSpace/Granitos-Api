namespace Granitos.Services.Domain.Documents;

public sealed class OrderDocument : MongoDocument
{
    public const string CollectionName = "orders";
    
    public OrderDocument(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy,
        DateTime? deletedAt, string? deletedBy, Dictionary<string, string> metadata, List<string>? tags,
        int totalProducts, decimal totalPrice) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt,
        deletedBy, metadata, tags)
    {
        TotalProducts = totalProducts;
        TotalPrice = totalPrice;
    }

    public int TotalProducts { get; private set; }
    public decimal TotalPrice { get; private set; }
}
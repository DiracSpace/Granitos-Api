namespace Granitos.Services.Domain.Entities;

public sealed class OrderProduct : Entity
{
    public OrderProduct(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, DateTime? deletedAt,
        IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags, Guid orderId, Guid productId,
        string productName, int quantity, decimal unitPrice) : base(id, createdAt, createdBy, updatedAt, deletedAt,
        metadata, tags)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
}
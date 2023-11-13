namespace Granitos.Services.Domain.Entities;

public sealed class Order : Entity
{
    public Order(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, DateTime? deletedAt,
        IReadOnlyDictionary<string, string> metadata, IReadOnlyList<string> tags, int totalProducts, decimal totalPrice)
        : base(id, createdAt, createdBy, updatedAt, deletedAt, metadata, tags)
    {
        TotalProducts = totalProducts;
        TotalPrice = totalPrice;
    }

    public int TotalProducts { get; private set; }
    public decimal TotalPrice { get; private set; }
}
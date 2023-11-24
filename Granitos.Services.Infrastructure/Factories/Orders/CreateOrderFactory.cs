using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Factories;

namespace Granitos.Services.Infrastructure.Factories.Orders;

public class CreateOrderFactory : ICreateEntityFactory<Order>
{
    private readonly int _totalProducts;
    private readonly decimal _totalPrice;
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly IReadOnlyList<string> _tags;

    public CreateOrderFactory(int totalProducts, decimal totalPrice, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags)
    {
        _totalProducts = totalProducts;
        _totalPrice = totalPrice;
        _metadata = metadata;
        _tags = tags;
    }

    public Order Create()
    {
        return new Order(
            Guid.NewGuid(),
            createdAt: DateTime.UtcNow,
            createdBy: string.Empty,
            updatedAt: DateTime.UtcNow,
            updatedBy: string.Empty,
            deletedAt: default,
            deletedBy: string.Empty,
            _metadata,
            _tags,
            _totalProducts,
            _totalPrice);
    }
}
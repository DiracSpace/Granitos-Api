using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Factories;

namespace Granitos.Services.Infrastructure.Factories.Orders;

public class UpdateOrderFactory : IUpdateEntityFactory<Order>
{
    private readonly int _totalProducts;
    private readonly decimal _totalPrice;
    private readonly IReadOnlyDictionary<string, string> _metadata;
    private readonly IReadOnlyList<string> _tags;

    public UpdateOrderFactory(int totalProducts, decimal totalPrice, IReadOnlyDictionary<string, string> metadata,
        IReadOnlyList<string> tags)
    {
        _totalProducts = totalProducts;
        _totalPrice = totalPrice;
        _metadata = metadata;
        _tags = tags;
    }

    public void Update(Order entity)
    {
        entity.SetTotalPrice(_totalPrice);
        entity.SetTotalProducts(_totalProducts);
        entity.SetMetadata(_metadata);
        entity.SetTags(_tags);
    }
}
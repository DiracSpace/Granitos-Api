using Granitos.Services.Domain.Entities;
using MediatR;

namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class GetProductCategoryByIdQuery : IRequest<ProductCategory>
{
    public GetProductCategoryByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
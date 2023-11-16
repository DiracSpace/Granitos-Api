using Granitos.Services.Domain.Cqrs.ProductCategories;
using Granitos.Services.Domain.Entities;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

internal sealed class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, ProductCategory>
{
    public Task<ProductCategory> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
using Granitos.Services.Domain.Cqrs.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

internal sealed class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Guid> 
{
    public Task<Guid> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
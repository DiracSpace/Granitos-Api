using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using MediatR;

namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class GetProductCategoriesQuery : IRequest<PagedResult<ProductCategory>>
{
    
}
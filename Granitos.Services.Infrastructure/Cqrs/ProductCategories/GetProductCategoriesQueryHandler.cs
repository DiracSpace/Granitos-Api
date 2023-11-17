using FluentValidation;
using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

public sealed record GetProductCategoriesQuery(
        int PageIndex,
        int PageSize)
    : IRequest<PagedResult<ProductCategory>>;

internal sealed class
    GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, PagedResult<ProductCategory>>
{
    private readonly IProductCategoriesRepository _repository;

    public GetProductCategoriesQueryHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ProductCategory>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        new GetProductCategoriesQueryValidator().ValidateAndThrow(request);

        return await _repository.GetAsync(
            request.PageIndex,
            request.PageSize);
    }
}

internal sealed class GetProductCategoriesQueryValidator : AbstractValidator<GetProductCategoriesQuery>
{
    public GetProductCategoriesQueryValidator()
    {
        RuleFor(r => r.PageIndex)
            .NotNull();
        
        RuleFor(r => r.PageSize)
            .NotNull();
    }
}

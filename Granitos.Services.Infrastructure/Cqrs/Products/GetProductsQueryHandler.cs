using FluentValidation;
using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Products;

public sealed record GetProductsQuery(
    int PageIndex,
    int PageSize,
    string? OrderBy,
    string? Filter) : IRequest<PagedResult<Product>>;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<Product>>
{
    private readonly ILogger<GetProductsQueryHandler> _logger;
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(ILogger<GetProductsQueryHandler> logger, IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<PagedResult<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        new GetProductsQueryValidator().ValidateAndThrow(request);

        return await _repository.GetAsync(
            request.PageIndex,
            request.PageSize);
    }
}

internal sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(r => r.PageIndex)
            .NotNull();

        RuleFor(r => r.PageSize)
            .NotNull();
    }
}
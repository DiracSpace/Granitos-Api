using FluentValidation;
using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Orders;

public sealed record GetOrdersQuery(
    int PageIndex,
    int PageSize,
    string? OrderBy,
    string? Filter) : IRequest<PagedResult<Order>>;

internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResult<Order>>
{
    private readonly ILogger<GetOrdersQueryHandler> _logger;
    private readonly IOrdersRepository _repository;

    public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IOrdersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<PagedResult<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        new GetOrdersQueryValidator().ValidateAndThrow(request);

        return await _repository.GetAsync(
            request.PageIndex,
            request.PageSize);
    }
}

internal sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(r => r.PageIndex)
            .NotNull();

        RuleFor(r => r.PageSize)
            .NotNull();
    }
}
using FluentValidation;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Orders;

public sealed record GetOrderByIdQuery(
    Guid Id) : IRequest<Order>;

internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;
    private readonly IOrdersRepository _repository;

    public GetOrderByIdQueryHandler(ILogger<GetOrderByIdQueryHandler> logger, IOrdersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        new GetOrderByIdQueryValidator().ValidateAndThrow(request);

        // TODO: add validation for existence
        var document = await _repository.GetByIdAsync(request.Id);

        return document;
    }
}

internal sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
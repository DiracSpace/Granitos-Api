using FluentValidation;
using Granitos.Services.Infrastructure.Factories.Orders;
using Granitos.Services.Infrastructure.Repositories.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Orders;

public sealed record CreateOrderCommand(
    int TotalProducts,
    decimal TotalPrice,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest<Guid>;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IOrdersRepository _repository;

    public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrdersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        new CreateOrderCommandValidator().ValidateAndThrow(request);

        var newOrder = new CreateOrderFactory(
                request.TotalProducts,
                request.TotalPrice,
                request.Metadata,
                request.Tags)
            .Create();

        var createdOrder = await _repository.CreateAsync(newOrder);

        return createdOrder.Id;
    }
}

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(r => r.TotalProducts)
            .NotEmpty()
            .NotNull();
        
        RuleFor(r => r.TotalPrice)
            .NotEmpty()
            .NotNull();
        
        // TODO: move to separate validator to prevent repeating code
        When(r => r.Metadata.Count > 0, () =>
        {
            RuleFor(r => r.Metadata)
                .Must(metadata => metadata.Select(m => m.Key).All(k => !string.IsNullOrWhiteSpace(k)));

            RuleFor(r => r.Metadata)
                .Must(metadata => metadata.Select(m => m.Key).ToHashSet().Count == metadata.Count);
        });
    }
}
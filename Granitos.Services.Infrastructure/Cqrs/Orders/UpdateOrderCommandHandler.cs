using FluentValidation;
using Granitos.Services.Infrastructure.Factories.Orders;
using Granitos.Services.Infrastructure.Repositories.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Orders;

public sealed record UpdateOrderCommand(
    Guid Id,
    int TotalProducts,
    decimal TotalPrice,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest;

internal sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IOrdersRepository _repository;

    public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrdersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        new UpdateOrderCommandValidator().ValidateAndThrow(request);

        var order = await _repository.GetByIdAsync(request.Id);
        
        new UpdateOrderFactory(
                request.TotalProducts,
                request.TotalPrice,
                request.Metadata,
                request.Tags)
            .Update(order);

        await _repository.UpdateAsync(order);
    }
}

internal sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
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
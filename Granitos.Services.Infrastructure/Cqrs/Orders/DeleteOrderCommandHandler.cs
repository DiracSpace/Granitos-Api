using FluentValidation;
using Granitos.Services.Infrastructure.Repositories.Orders;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Orders;

public sealed record DeleteOrderCommand(
    Guid Id) : IRequest;

internal sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ILogger<DeleteOrderCommandHandler> _logger;
    private readonly IOrdersRepository _repository;

    public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrdersRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        new DeleteOrderCommandValidator().ValidateAndThrow(request);

        await _repository.DeleteAsync(request.Id);
    }
}

internal sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
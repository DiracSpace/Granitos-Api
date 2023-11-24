using FluentValidation;
using Granitos.Services.Infrastructure.Repositories.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Products;

public sealed record DeleteProductCommand(
    Guid Id) : IRequest;

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly ILogger<DeleteProductCommandHandler> _logger;
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        new DeleteProductCommandValidator().ValidateAndThrow(request);

        await _repository.DeleteAsync(request.Id);
    }
}

internal sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
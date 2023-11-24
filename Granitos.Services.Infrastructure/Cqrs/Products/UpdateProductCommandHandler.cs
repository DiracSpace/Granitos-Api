using FluentValidation;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Factories.Products;
using Granitos.Services.Infrastructure.Repositories.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Products;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Code,
    string Description,
    string ImageUrl,
    decimal UnitPrice,
    int UnitInStock,
    ProductCategory Category,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly ILogger<UpdateProductCommandHandler> _logger;
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        new UpdateProductCommandValidator().ValidateAndThrow(request);

        var product = await _repository.GetByIdAsync(request.Id);

        new UpdateProductFactory(
                request.Name,
                request.Code,
                request.Description,
                request.ImageUrl,
                request.UnitPrice,
                request.UnitInStock,
                request.Category,
                request.Metadata,
                request.Tags)
            .Update(product);

        await _repository.UpdateAsync(product);
    }
}

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(r => r.Code)
            .NotEmpty()
            .NotNull();

        RuleFor(r => r.UnitPrice)
            .NotEmpty()
            .NotNull();

        RuleFor(r => r.UnitInStock)
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
using FluentValidation;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Factories.Products;
using Granitos.Services.Infrastructure.Repositories.Products;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.Products;

public sealed record CreateProductCommand(
    string Name,
    string Code,
    string Description,
    string ImageUrl,
    decimal UnitPrice,
    int UnitInStock,
    ProductCategory Category,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest<Guid>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        new CreateProductCommandValidator().ValidateAndThrow(request);

        var newProduct = new CreateProductFactory(
                request.Name,
                request.Code,
                request.Description,
                request.ImageUrl,
                request.UnitPrice,
                request.UnitInStock,
                request.Category,
                request.Metadata,
                request.Tags)
            .Create();

        var createdProduct = await _repository.CreateAsync(newProduct);
        return createdProduct.Id;
    }
}

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
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
    }
}
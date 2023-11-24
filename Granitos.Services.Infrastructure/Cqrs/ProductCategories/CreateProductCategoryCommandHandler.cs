using FluentValidation;
using Granitos.Services.Infrastructure.Factories.ProductCategories;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

public sealed record CreateProductCategoryCommand(
    string Name,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest<Guid>;

internal sealed class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Guid>
{
    private readonly IProductCategoriesRepository _repository;

    public CreateProductCategoryCommandHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        new CreateProductCategoryCommandValidator().ValidateAndThrow(request);

        var newProductCategory = new CreateProductCategoryFactory(
                request.Metadata,
                request.Tags,
                request.Name)
            .Create();

        var createdProductCategory = await _repository.CreateAsync(newProductCategory);
        return createdProductCategory.Id;
    }
}

internal sealed class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(r => r.Name)
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
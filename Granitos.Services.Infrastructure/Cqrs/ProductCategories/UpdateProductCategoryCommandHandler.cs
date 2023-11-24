using FluentValidation;
using Granitos.Services.Infrastructure.Factories.ProductCategories;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

public sealed record UpdateProductCategoryCommand(
    Guid Id,
    string Name,
    Dictionary<string, string> Metadata,
    List<string> Tags) : IRequest;

internal sealed class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand>
{
    private readonly IProductCategoriesRepository _repository;

    public UpdateProductCategoryCommandHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        new UpdateProductCategoryCommandValidator().ValidateAndThrow(request);

        var productCategory = await _repository.GetByIdAsync(request.Id);

        new UpdateProductCategoryFactory(
            request.Metadata,
            request.Tags,
            request.Name).Update(productCategory);

        await _repository.UpdateAsync(productCategory);
    }
}

internal sealed class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
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
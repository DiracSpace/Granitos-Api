using FluentValidation;
using Granitos.Services.Domain.Cqrs.ProductCategories;

namespace Granitos.Services.Domain.Validators.ProductCategories;

public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .NotNull()
            ;

        When(r => r.Metadata.Count > 0, () =>
        {
            RuleFor(r => r.Metadata)
                .Must(metadata => metadata.Select(m => m.Key).All(k => !string.IsNullOrWhiteSpace(k)));

            RuleFor(r => r.Metadata)
                .Must(metadata => metadata.Select(m => m.Key).ToHashSet().Count == metadata.Count);
        });
    }
}
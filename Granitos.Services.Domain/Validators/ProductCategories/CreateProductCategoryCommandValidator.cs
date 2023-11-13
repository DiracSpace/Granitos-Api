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
    }
}
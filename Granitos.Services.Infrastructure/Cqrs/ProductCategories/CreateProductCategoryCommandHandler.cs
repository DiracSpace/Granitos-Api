using FluentValidation;
using Granitos.Services.Domain.Cqrs.ProductCategories;
using Granitos.Services.Domain.Factories.ProductCategories;
using Granitos.Services.Domain.Validators.ProductCategories;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

internal sealed class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Guid>
{
    private readonly IProductCategoriesRepository _repository;

    public CreateProductCategoryCommandHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        await new CreateProductCategoryCommandValidator().ValidateAndThrowAsync(request,
            cancellationToken: cancellationToken);

        var newProductCategory = new CreateProductCategoryFactory(
                request.Metadata,
                request.Tags,
                request.Name)
            .Create();

        var createdProductCategory = await _repository.CreateAsync(newProductCategory);
        return createdProductCategory.Id;
    }
}
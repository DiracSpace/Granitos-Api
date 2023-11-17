using FluentValidation;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

public sealed record GetProductCategoryByIdQuery(
    Guid Id) : IRequest<ProductCategory>;

internal sealed class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, ProductCategory>
{
    private readonly IProductCategoriesRepository _repository;

    public GetProductCategoryByIdQueryHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductCategory> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        new GetProductCategoryByIdQueryValidator().ValidateAndThrow(request);

        // TODO: add validation for existence
        var document = await _repository.GetByIdAsync(request.Id);

        return document;
    }
}

internal sealed class GetProductCategoryByIdQueryValidator : AbstractValidator<GetProductCategoryByIdQuery>
{
    public GetProductCategoryByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
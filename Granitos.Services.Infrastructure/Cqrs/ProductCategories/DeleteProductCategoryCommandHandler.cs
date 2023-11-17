using FluentValidation;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using MediatR;

namespace Granitos.Services.Infrastructure.Cqrs.ProductCategories;

public sealed record DeleteProductCategoryCommand(
    Guid Id) : IRequest;

internal sealed class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand>
{
    private readonly IProductCategoriesRepository _repository;

    public DeleteProductCategoryCommandHandler(IProductCategoriesRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        new DeleteProductCategoryCommandValidator().ValidateAndThrow(request);

        await _repository.DeleteAsync(request.Id);
    }
}

internal sealed class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
{
    public DeleteProductCategoryCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
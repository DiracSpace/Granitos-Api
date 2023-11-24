using FluentValidation;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Repositories.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Granitos.Services.Infrastructure.Cqrs.Products;

public sealed record GetProductByIdQuery(
    Guid Id) : IRequest<Product>;

internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly ILogger<GetProductByIdQueryHandler> _logger;
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(ILogger<GetProductByIdQueryHandler> logger, IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        new GetProductByIdQueryValidator().ValidateAndThrow(request);

        // TODO: add validation for existence
        var document = await _repository.GetByIdAsync(request.Id);

        return document;
    }
}

internal sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .NotEmpty();
    }
}
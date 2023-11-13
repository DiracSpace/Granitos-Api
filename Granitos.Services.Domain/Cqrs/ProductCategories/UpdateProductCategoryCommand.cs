using MediatR;

namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class UpdateProductCategoryCommand : IRequest
{
    public UpdateProductCategoryCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
}
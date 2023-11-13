using MediatR;

namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class CreateProductCategoryCommand : IRequest<Guid>
{
    public CreateProductCategoryCommand(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}
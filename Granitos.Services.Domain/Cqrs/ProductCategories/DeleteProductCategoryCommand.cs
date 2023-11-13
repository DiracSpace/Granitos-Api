namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class DeleteProductCategoryCommand
{
    public DeleteProductCategoryCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
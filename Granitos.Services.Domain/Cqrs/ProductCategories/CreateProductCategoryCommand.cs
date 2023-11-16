using MediatR;

namespace Granitos.Services.Domain.Cqrs.ProductCategories;

public class CreateProductCategoryCommand : IRequest<Guid>
{
    public CreateProductCategoryCommand(string name, Dictionary<string, string> metadata, List<string> tags)
    {
        Name = name;
        Metadata = metadata;
        Tags = tags;
    }

    public string Name { get; private set; }
    public Dictionary<string, string> Metadata { get; set; }
    public List<string> Tags { get; set; }
}
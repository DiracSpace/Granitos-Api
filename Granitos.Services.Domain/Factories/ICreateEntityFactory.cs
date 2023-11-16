namespace Granitos.Services.Domain.Factories;

public interface ICreateEntityFactory<out TEntity>
{
    TEntity Create();
}
namespace Granitos.Services.Domain.Factories;

public interface IUpdateEntityFactory<in TEntity>
{
    void Update(TEntity entity);
}
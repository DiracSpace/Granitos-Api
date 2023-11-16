using Granitos.Common.Mongo.Pagination.SkipLimitPattern;

namespace Granitos.Services.Infrastructure.Repositories;

public interface IRepository<T> 
    where T : class
{
    Task<T> CreateAsync(T entity);
    Task<PagedResult<T>> GetAsync(int pageIndex, int pageSize);
    Task<T> GetByIdAsync(Guid id);
    Task<T?> GetByIdOrDefaultAsync(Guid id);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
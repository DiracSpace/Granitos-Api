namespace Granitos.Common.Mongo.Pagination.Abstractions;

public interface IPagination<TDocument>
{
    // TODO Add support for filtering
    Task<IPagedResult<TDocument>> GetAsync(int pageIndex, int pageSize);
}
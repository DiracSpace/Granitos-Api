using Granitos.Common.Mongo.Pagination.Abstractions;

namespace Granitos.Common.Mongo.Pagination.SkipLimitPattern;

public class PagedResult<T> : IPagedResult<T>
{
    public PagedResult(int pageIndex, int pageSize, long totalCount, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        Data = data;
    }

    public int PageIndex { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public IReadOnlyList<T> Data { get; }
}
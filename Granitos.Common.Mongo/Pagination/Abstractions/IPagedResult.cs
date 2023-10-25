namespace Granitos.Common.Mongo.Pagination.Abstractions;

public interface IPagedResult<TDocument>
{
    int PageIndex { get; }
    int PageSize { get; }
    long TotalCount { get; }
    IReadOnlyList<TDocument> Data { get; }
}
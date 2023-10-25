using Granitos.Common.Mongo.Pagination.Abstractions;
using MongoDB.Driver;

namespace Granitos.Common.Mongo.Pagination.SkipLimitPattern;

internal interface ISkipLimitPagination<TDocument> : IPagination<TDocument>
{
}

internal class SkipLimitPagination<TDocument> : ISkipLimitPagination<TDocument>
{
    private readonly IMongoCollection<TDocument> _collection;

    public SkipLimitPagination(IMongoCollection<TDocument> collection)
    {
        _collection = collection;
    }

    public async Task<IPagedResult<TDocument>> GetAsync(int pageIndex, int pageSize)
    {
        var totalCount = await _collection
            .CountDocumentsAsync(_ => true);

        var results = await _collection
            .Find(_ => true)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<TDocument>(
            pageIndex,
            pageSize,
            totalCount,
            results);
    }
}
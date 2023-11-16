using Granitos.Common.Mongo.Pagination.Abstractions;

namespace Granitos.Common.Mongo.Pagination.BucketPattern;

internal interface IBucketPatternPagination<TDocument> : IPagination<TDocument>
{
}

internal class BucketPatternPagination<TDocument> : IBucketPatternPagination<TDocument>
{
    public Task<IPagedResult<TDocument>> GetAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }
}
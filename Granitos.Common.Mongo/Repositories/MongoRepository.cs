using System.Linq.Expressions;
using Granitos.Common.Mongo.Exceptions;
using Granitos.Common.Mongo.Pagination.Abstractions;
using Granitos.Common.Mongo.Repositories.Abstractions;
using MongoDB.Driver;

namespace Granitos.Common.Mongo.Repositories;

internal class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IMongoDocument
{
    private readonly IMongoCollection<TDocument> _collection;
    private readonly IPagination<TDocument> _pagination;

    public MongoRepository(IMongoCollection<TDocument> collection, IPagination<TDocument> pagination)
    {
        _collection = collection;
        _pagination = pagination;
    }

    public async Task<TDocument> CreateAsync(TDocument document)
    {
        if (document.Id == Guid.Empty)
            document.Id = Guid.NewGuid();

        document.CreatedAt = DateTime.UtcNow;
        document.CreatedBy = document.CreatedBy; // TODO Figure out a neat way to set CreatedBy

        await _collection.InsertOneAsync(document);

        return document;
    }

    public async Task<IPagedResult<TDocument>> GetAsync(
        int pageIndex = 0,
        int pageSize = 10)
    {
        return await _pagination.GetAsync(pageIndex, pageSize);
    }

    public async Task<TDocument> GetOneAsync(Expression<Func<TDocument, bool>> filter)
    {
        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException<TDocument>();
    }

    public async Task<TDocument> GetOneAsync(Guid id)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException<TDocument>(id);
    }

    public async Task<TDocument?> GetOneOrDefaultAsync(Expression<Func<TDocument, bool>> filter)
    {
        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<TDocument?> GetOneOrDefaultAsync(Guid id)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(TDocument document)
    {
        document.UpdatedAt = DateTime.UtcNow;
        document.UpdatedBy = document.UpdatedBy; // TODO Figure out a neat way to set UpdatedBy

        await _collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document);
    }

    public async Task DeleteAsync(Guid id)
    {
        var document = await GetOneAsync(id);
        
        document.DeletedAt = DateTime.UtcNow;
        document.DeletedBy = document.DeletedBy; // TODO Figure out a neat way to set DeletedBy

        await _collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document);
    }

    public async Task PurgeAsync(Guid id)
    {
        await _collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}
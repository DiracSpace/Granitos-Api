using System.Linq.Expressions;
using Granitos.Common.Mongo.Pagination.Abstractions;

namespace Granitos.Common.Mongo.Repositories.Abstractions;

public interface IMongoRepository<TDocument>
    where TDocument : IMongoDocument
{
    Task<TDocument> CreateAsync(TDocument document);
    
    Task<IPagedResult<TDocument>> GetAsync(
        int pageIndex = 0,
        int pageSize = 10);

    Task<TDocument> GetOneAsync(Expression<Func<TDocument, bool>> filter);
    Task<TDocument> GetOneAsync(Guid id);
    Task<TDocument?> GetOneOrDefaultAsync(Expression<Func<TDocument, bool>> filter);
    Task<TDocument?> GetOneOrDefaultAsync(Guid id);
    Task UpdateAsync(TDocument document);

    /// <summary>
    /// Applies a soft delete to the document. The document can be recovered later if desired.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Applies a hard delete to the document. The document will be lost permanently.
    /// </summary>
    Task PurgeAsync(Guid id);
}
using Granitos.Common.Mongo.Pagination.Abstractions;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Mongo.Pagination.BucketPattern.DependencyInjection;

public static class BucketPatternPaginationDiExtensions
{
    public static IServiceCollection AddMongoBucketPatternPagination<TDocument>(this IServiceCollection services)
        where TDocument : IMongoDocument
    {
        return services.AddScoped<IPagination<TDocument>, BucketPatternPagination<TDocument>>();
    }
}
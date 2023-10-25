using Granitos.Common.Mongo.Pagination.Abstractions;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Mongo.Pagination.SkipLimitPattern.DependencyInjection;

public static class SkipLimitPaginationDIExtensions
{
    public static IServiceCollection AddMongoSkipLimitPagination<TDocument>(this IServiceCollection services)
        where TDocument : IMongoDocument
    {
        return services.AddScoped<IPagination<TDocument>, SkipLimitPagination<TDocument>>();
    }
}
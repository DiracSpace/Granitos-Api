using Granitos.Common.Extensions;
using Granitos.Common.Mongo.DependencyInjection;
using Granitos.Common.Mongo.Pagination.BucketPattern.DependencyInjection;
using Granitos.Common.Mongo.Pagination.SkipLimitPattern.DependencyInjection;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Granitos.Services.Domain.Documents;
using Granitos.Services.Infrastructure.Mapper.AutoMappers.DependencyInjection;
using Granitos.Services.Infrastructure.Mapper.Mapsters.DependencyInjection;
using Granitos.Services.Infrastructure.Repositories.Orders;
using Granitos.Services.Infrastructure.Repositories.ProductCategories;
using Granitos.Services.Infrastructure.Repositories.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Services.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCqrs()
            .AddMongo(configuration)
            .AddEntityRepositories()
            .AddMapper(configuration);
    }

    private static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        return services
            .AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<IInfrastructureMarker>());
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("Mongo:ConnectionString");
        var databaseName = configuration.GetRequiredString("Mongo:DatabaseName");
        var paginationStrategy = configuration.GetRequiredString("Mongo:Pagination:Strategy");

        return services
                .AddMongoDatabase(new MongoOptions
                {
                    ConnectionString = connectionString,
                    DatabaseName = databaseName
                })
                .AddMongoDocumentServices<ProductCategoryDocument>(ProductCategoryDocument.CollectionName,
                    paginationStrategy)
                .AddMongoDocumentServices<ProductDocument>(ProductDocument.CollectionName,
                    paginationStrategy)
                .AddMongoDocumentServices<OrderDocument>(OrderDocument.CollectionName,
                    paginationStrategy)
            ;
    }

    private static IServiceCollection AddMongoDocumentServices<TDocument>(
        this IServiceCollection services,
        string collectionName,
        string paginationStrategy)
        where TDocument : IMongoDocument
    {
        return services
            .AddMongoRepository<TDocument>(collectionName)
            .AddMongoPagination<TDocument>(paginationStrategy);
    }

    private static IServiceCollection AddMongoPagination<TDocument>(
        this IServiceCollection services,
        string paginationStrategy)
        where TDocument : IMongoDocument
    {
        return paginationStrategy switch
        {
            "BucketPattern" => services.AddMongoBucketPatternPagination<TDocument>(),
            "SkipLimit" => services.AddMongoSkipLimitPagination<TDocument>(),
            _ => throw new InvalidOperationException($"""Unknown pagination strategy "{paginationStrategy}".""")
        };
    }

    private static IServiceCollection AddEntityRepositories(this IServiceCollection services)
    {
        return services
                .AddTransient<IProductCategoriesRepository, ProductCategoriesRepository>()
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IOrdersRepository, OrdersRepository>()
            ;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services, IConfiguration configuration)
    {
        var mapperProvider = configuration.GetRequiredString("Mapper:Provider");

        return mapperProvider switch
        {
            "AutoMapper" => services.RegisterAutoMapper(),
            "Mapster" => services.RegisterMapster(),
            _ => throw new InvalidOperationException($"""
                                                      Unknown mapper provider: "{mapperProvider}"
                                                      """)
        };
    }
}
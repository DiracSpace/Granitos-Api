using Granitos.Common.Mongo.Repositories;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Granitos.Common.Mongo.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, MongoOptions options)
    {
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);

        return services
                .AddSingleton<IMongoClient>(client)
                .AddSingleton(database)
            ;
    }

    public static IServiceCollection AddMongoRepository<TDocument>(this IServiceCollection services,
        string collectionName)
        where TDocument : IMongoDocument
    {
        return services
            .AddMongoCollection<TDocument>(collectionName)
            .AddScoped<IMongoRepository<TDocument>, MongoRepository<TDocument>>();
    }

    private static IServiceCollection AddMongoCollection<TDocument>(this IServiceCollection services,
        string collectionName)
    {
        return services.AddTransient<IMongoCollection<TDocument>>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<TDocument>(collectionName);
        });
    }
}
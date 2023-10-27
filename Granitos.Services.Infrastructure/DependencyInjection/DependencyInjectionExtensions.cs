using Granitos.Common.Extensions;
using Granitos.Common.Mongo.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Services.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .AddCqrs()
                .AddMongo(configuration)
            ;
    }

    private static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        return services
            .AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<IInfrastructureMarker>());
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetRequiredString("mongo:connectionString");
        var databaseName = configuration.GetRequiredString("mongo:databaseName");

        return services
                .AddMongoDatabase(new MongoOptions
                {
                    ConnectionString = connectionString,
                    DatabaseName = databaseName
                })
            ;
    }
}
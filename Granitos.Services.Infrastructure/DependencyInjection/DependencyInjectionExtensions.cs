using Granitos.Common.Extensions;
using Granitos.Common.Mongo.DependencyInjection;
using Granitos.Services.Infrastructure.Mapper.AutoMappers.DependencyInjection;
using Granitos.Services.Infrastructure.Mapper.Mapsters.DependencyInjection;
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
        var connectionString = configuration.GetRequiredString("mongo:connectionString");
        var databaseName = configuration.GetRequiredString("mongo:databaseName");

        return services
            .AddMongoDatabase(new MongoOptions
            {
                ConnectionString = connectionString,
                DatabaseName = databaseName
            });
    }
    
    private static IServiceCollection AddMapper(this IServiceCollection services, IConfiguration configuration)
    {
        var mapperProvider = configuration.GetRequiredString("Mapper:Provider");

        return mapperProvider switch
        {
            "AutoMapper" => services.RegisterAutoMapper(),
            "Mapster" => services.RegisterMapster(),
            _ => throw new InvalidOperationException($@"Unknown mapper provider: ""{mapperProvider}"""),
        };
    }
}
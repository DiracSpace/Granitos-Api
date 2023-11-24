using Granitos.Api.HealthChecks.Dependencies;
using Granitos.Api.HealthChecks.Self;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Granitos.Api.HealthChecks.DependencyInjection;

public static class HealthChecksDependencyInjection
{
    public static void AddCustomHealthCheckServices(this IServiceCollection services)
    {
        services
            .ConfigureHealthCheckPublisherOptions()
            .AddSelfHealthCheck();
    }

    private static IServiceCollection ConfigureHealthCheckPublisherOptions(this IServiceCollection services)
    {
        return services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromHours(24);
            options.Period = TimeSpan.FromHours(24);
        });
    }

    private static IServiceCollection AddSelfHealthCheck(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<SelfHealthCheck>(SelfHealthCheck.Name)
            .AddDependenciesHealthChecks();

        return services;
    }

    private static void AddDependenciesHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
    {
        var tags = new[] { DependencyHealthCheck.Tag };

        healthChecksBuilder
            .AddCheck<MongoHealthCheck>(
                MongoHealthCheck.Name, tags: tags);
    }

    public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapReadinessProbeHealthCheckEndpoint()
            .MapLivenessProbeHealthCheckEndpoint()
            .MapDependenciesHealthCheckEndpoint();
    }

    private static IEndpointRouteBuilder MapReadinessProbeHealthCheckEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = x => x.Name == SelfHealthCheck.Name,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return builder;
    }

    private static IEndpointRouteBuilder MapLivenessProbeHealthCheckEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = x => x.Name == SelfHealthCheck.Name
        });

        return builder;
    }

    private static void MapDependenciesHealthCheckEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("/health/dependencies", new HealthCheckOptions
        {
            Predicate = x => x.Tags.Contains(DependencyHealthCheck.Tag),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
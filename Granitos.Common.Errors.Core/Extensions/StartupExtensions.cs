using Granitos.Common.Errors.Core.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.Core.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddProblemDetailsFactory<TException, TFactory>(this IServiceCollection services)
        where TException : Exception
        where TFactory : class, IProblemDetailsFactory<TException>
    {
        return services
                .AddSingleton<IProblemDetailsFactory, TFactory>() // Register with non-generic to be able to retrieve via "IProblemDetailsFactory" token
                .AddSingleton<IProblemDetailsFactory<TException>, TFactory>() // Register with generic to be able to retrieve via "IProblemDetailsFactory<TException>" token
            ;
    }

    public static IServiceCollection AddProblemDetailsResolver(this IServiceCollection services)
    {
        return services
            .AddSingleton<IProblemDetailsResolver, ProblemDetailsResolver>();
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Testing.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static TService BuildStandaloneService<TService>(this IServiceCollection services)
        where TService : notnull
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<TService>();
    }
}
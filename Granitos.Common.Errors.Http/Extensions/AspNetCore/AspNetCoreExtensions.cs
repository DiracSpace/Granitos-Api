using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Common.Errors.Http.Extensions.AspNetCore;

public static class AspNetCoreExtensions
{
    public static IServiceCollection AddHttpExceptionFilterOptions(this IServiceCollection services,
        IHttpExceptionFilterOptions options)
    {
        return services
                .AddSingleton(options)
            ;
    }
}
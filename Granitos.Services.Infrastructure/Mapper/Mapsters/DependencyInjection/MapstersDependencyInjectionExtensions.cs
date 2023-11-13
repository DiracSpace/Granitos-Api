using Granitos.Services.Infrastructure.Mapper.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Services.Infrastructure.Mapper.Mapsters.DependencyInjection;

internal static class MapstersDependencyInjectionExtensions
{
    public static IServiceCollection RegisterMapster(this IServiceCollection services)
    {
        return services
                .AddSingleton<IMapper, Mapper>()
            ;
    }
}
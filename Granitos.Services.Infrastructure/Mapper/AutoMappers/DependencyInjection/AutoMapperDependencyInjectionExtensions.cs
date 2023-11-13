using Granitos.Services.Infrastructure.Mapper.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Granitos.Services.Infrastructure.Mapper.AutoMappers.DependencyInjection;

internal static class AutoMapperDependencyInjectionExtensions
{
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        return services
                .AddAutoMapper(typeof(Mapper))
                .AddSingleton<IMapper, Mapper>()
            ;
    }
}
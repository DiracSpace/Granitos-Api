using Granitos.Services.Infrastructure.Mapper.Abstractions;
using Mapster;

namespace Granitos.Services.Infrastructure.Mapper.Mapsters;

internal sealed class Mapper : IMapper
{
    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return source.Adapt<TDestination>();
    }
}
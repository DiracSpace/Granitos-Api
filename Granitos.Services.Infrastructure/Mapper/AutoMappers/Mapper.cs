using Granitos.Services.Infrastructure.Mapper.Abstractions;

namespace Granitos.Services.Infrastructure.Mapper.AutoMappers;

internal sealed class Mapper : IMapper
{
    private readonly AutoMapper.IMapper _mapper;

    public Mapper(AutoMapper.IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
        => _mapper.Map<TDestination>(source);
}
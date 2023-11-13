namespace Granitos.Services.Infrastructure.Mapper.Abstractions;

public interface IMapper
{
    public TDestination Map<TDestination>(object source);
}
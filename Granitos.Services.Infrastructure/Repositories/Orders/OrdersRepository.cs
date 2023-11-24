using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Granitos.Services.Domain.Documents;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Repositories;
using Granitos.Services.Infrastructure.Mapper.Abstractions;

namespace Granitos.Services.Infrastructure.Repositories.Orders;

public interface IOrdersRepository : IRepository<Order>
{
    
}

internal sealed class OrdersRepository : IOrdersRepository
{
    private readonly IMongoRepository<OrderDocument> _repository;
    private readonly IMapper _mapper;

    public OrdersRepository(IMongoRepository<OrderDocument> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Order> CreateAsync(Order entity)
    {
        var createdDocument = await _repository.CreateAsync(_mapper.Map<OrderDocument>(entity));
        return _mapper.Map<Order>(createdDocument);
    }

    public async Task<PagedResult<Order>> GetAsync(int pageIndex, int pageSize)
    {
        var documents = await _repository.GetAsync(pageIndex, pageSize);

        return new PagedResult<Order>(
            documents.PageIndex,
            documents.PageSize,
            documents.TotalCount,
            documents.Data
                .Select(_mapper.Map<Order>)
                .ToList());
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        var document = await _repository.GetOneAsync(id);
        return _mapper.Map<Order>(document);
    }

    public async Task<Order?> GetByIdOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document is null ? null : _mapper.Map<Order>(document);
    }

    public async Task UpdateAsync(Order entity)
        => await _repository.UpdateAsync(_mapper.Map<OrderDocument>(entity));

    public async Task DeleteAsync(Guid id)
        => await _repository.DeleteAsync(id);

    public async Task<bool> ExistsAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document is not null;
    }
}
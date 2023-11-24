using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Granitos.Services.Domain.Documents;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Repositories;
using Granitos.Services.Infrastructure.Mapper.Abstractions;

namespace Granitos.Services.Infrastructure.Repositories.Products;

public interface IProductRepository : IRepository<Product>
{
}

internal sealed class ProductRepository : IProductRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<ProductDocument> _repository;

    public ProductRepository(IMongoRepository<ProductDocument> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        var createdDocument = await _repository.CreateAsync(_mapper.Map<ProductDocument>(entity));
        return _mapper.Map<Product>(createdDocument);
    }

    public async Task<PagedResult<Product>> GetAsync(int pageIndex, int pageSize)
    {
        var documents = await _repository.GetAsync(pageIndex, pageSize);

        return new PagedResult<Product>(
            documents.PageIndex,
            documents.PageSize,
            documents.TotalCount,
            documents.Data
                .Select(_mapper.Map<Product>)
                .ToList());
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var document = await _repository.GetOneAsync(id);
        return _mapper.Map<Product>(document);
    }

    public async Task<Product?> GetByIdOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document is null ? null : _mapper.Map<Product>(document);
    }

    public async Task UpdateAsync(Product entity)
    {
        await _repository.UpdateAsync(_mapper.Map<ProductDocument>(entity));
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document is not null;
    }
}
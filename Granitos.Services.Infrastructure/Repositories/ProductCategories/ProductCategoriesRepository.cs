using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Common.Mongo.Repositories.Abstractions;
using Granitos.Services.Domain.Documents;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Domain.Repositories;
using Granitos.Services.Infrastructure.Mapper.Abstractions;

namespace Granitos.Services.Infrastructure.Repositories.ProductCategories;

public interface IProductCategoriesRepository : IRepository<ProductCategory>
{
}

internal sealed class ProductCategoriesRepository : IProductCategoriesRepository
{
    private readonly IMapper _mapper;
    private readonly IMongoRepository<ProductCategoryDocument> _repository;

    public ProductCategoriesRepository(IMongoRepository<ProductCategoryDocument> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductCategory> CreateAsync(ProductCategory entity)
    {
        var createdDocument = await _repository.CreateAsync(_mapper.Map<ProductCategoryDocument>(entity));
        return _mapper.Map<ProductCategory>(createdDocument);
    }

    public async Task<PagedResult<ProductCategory>> GetAsync(int pageIndex, int pageSize)
    {
        var documents = await _repository.GetAsync(pageIndex, pageSize);

        return new PagedResult<ProductCategory>(
            documents.PageIndex,
            documents.PageSize,
            documents.TotalCount,
            documents.Data
                .Select(_mapper.Map<ProductCategory>)
                .ToList());
    }

    public async Task<ProductCategory> GetByIdAsync(Guid id)
    {
        var document = await _repository.GetOneAsync(id);
        return _mapper.Map<ProductCategory>(document);
    }

    public async Task<ProductCategory?> GetByIdOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document is null ? null : _mapper.Map<ProductCategory>(document);
    }

    public async Task UpdateAsync(ProductCategory entity)
    {
        await _repository.UpdateAsync(_mapper.Map<ProductCategoryDocument>(entity));
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
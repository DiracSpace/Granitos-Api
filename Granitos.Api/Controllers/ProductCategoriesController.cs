using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Cqrs.ProductCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Granitos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductCategoriesController : Controller
{
    private readonly IMediator _mediator;

    public ProductCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = nameof(CreateProductCategoryAsync))]
    public async Task<ActionResult> CreateProductCategoryAsync([FromBody] CreateProductCategoryCommand request)
    {
        var productCategoryId = await _mediator.Send(request);
        return CreatedAtRoute(nameof(GetProductCategoryByIdAsync), new { productCategoryId },
            new { productCategoryId });
    }

    [HttpGet("{productCategoryId:guid}", Name = nameof(GetProductCategoryByIdAsync))]
    public async Task<ProductCategory> GetProductCategoryByIdAsync([FromRoute] Guid productCategoryId)
        => await _mediator.Send(new GetProductCategoryByIdQuery(productCategoryId));

    [HttpGet(Name = nameof(GetProductCategoriesAsync))]
    public async Task<PagedResult<ProductCategory>> GetProductCategoriesAsync(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        [FromQuery] string? filter = null)
    {
        return await _mediator.Send(new GetProductCategoriesQuery(
            pageIndex,
            pageSize,
            orderBy,
            filter));
    }

    [HttpPut("{productCategoryId:guid}", Name = nameof(UpdateProductCategoryAsync))]
    public async Task<ActionResult> UpdateProductCategoryAsync(
        [FromRoute] Guid productCategoryId,
        [FromBody] UpdateProductCategoryCommand request)
    {
        if (productCategoryId != request.Id)
        {
            return BadRequest(new
            {
                message = "The id in the resource url does not match the id in the request body."
            });
        }

        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete("{productCategoryId:guid}", Name = nameof(DeleteProductCategoryAsync))]
    public async Task<ActionResult> DeleteProductCategoryAsync(
        [FromRoute] Guid productCategoryId)
    {
        await _mediator.Send(new DeleteProductCategoryCommand(productCategoryId));

        return NoContent();
    }
}
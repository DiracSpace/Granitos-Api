using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Cqrs.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Granitos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = nameof(CreateProductAsync))]
    public async Task<ActionResult> CreateProductAsync([FromBody] CreateProductCommand request)
    {
        var productId = await _mediator.Send(request);
        return CreatedAtRoute(nameof(GetProductByIdAsync), new { productId },
            new { productId });
    }

    [HttpGet("{productId:guid}", Name = nameof(GetProductByIdAsync))]
    public async Task<Product> GetProductByIdAsync([FromRoute] Guid productId)
    {
        return await _mediator.Send(new GetProductByIdQuery(productId));
    }

    [HttpGet(Name = nameof(GetProductsAsync))]
    public async Task<PagedResult<Product>> GetProductsAsync(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        [FromQuery] string? filter = null)
    {
        return await _mediator.Send(new GetProductsQuery(
            pageIndex,
            pageSize,
            orderBy,
            filter));
    }

    [HttpPut("{productId:guid}", Name = nameof(UpdateProductAsync))]
    public async Task<ActionResult> UpdateProductAsync(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductCommand request)
    {
        if (productId != request.Id)
            return BadRequest(new
            {
                message = "The id in the resource url does not match the id in the request body."
            });

        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete("{productId:guid}", Name = nameof(DeleteProductAsync))]
    public async Task<ActionResult> DeleteProductAsync(
        [FromRoute] Guid productId)
    {
        await _mediator.Send(new DeleteProductCommand(productId));

        return NoContent();
    }
}
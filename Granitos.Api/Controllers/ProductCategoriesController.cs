using Granitos.Services.Domain.Cqrs.ProductCategories;
using Granitos.Services.Domain.Entities;
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
}
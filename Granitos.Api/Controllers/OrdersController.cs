using Granitos.Common.Mongo.Pagination.SkipLimitPattern;
using Granitos.Services.Domain.Entities;
using Granitos.Services.Infrastructure.Cqrs.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Granitos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController : Controller
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = nameof(CreateOrderAsync))]
    public async Task<ActionResult> CreateOrderAsync([FromBody] CreateOrderCommand request)
    {
        var orderId = await _mediator.Send(request);
        return CreatedAtRoute(nameof(GetOrderByIdAsync), new { orderId },
            new { orderId });
    }

    [HttpGet("{orderId:guid}", Name = nameof(GetOrderByIdAsync))]
    public async Task<Order> GetOrderByIdAsync([FromRoute] Guid orderId)
        => await _mediator.Send(new GetOrderByIdQuery(orderId));

    [HttpGet(Name = nameof(GetOrdersAsync))]
    public async Task<PagedResult<Order>> GetOrdersAsync(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        [FromQuery] string? filter = null)
    {
        return await _mediator.Send(new GetOrdersQuery(
            pageIndex,
            pageSize,
            orderBy,
            filter));
    }

    [HttpPut("{orderId:guid}", Name = nameof(UpdateOrderAsync))]
    public async Task<ActionResult> UpdateOrderAsync(
        [FromRoute] Guid orderId,
        [FromBody] UpdateOrderCommand request)
    {
        if (orderId != request.Id)
        {
            return BadRequest(new
            {
                message = "The id in the resource url does not match the id in the request body."
            });
        }

        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete("{orderId:guid}", Name = nameof(DeleteOrderAsync))]
    public async Task<ActionResult> DeleteOrderAsync(
        [FromRoute] Guid orderId)
    {
        await _mediator.Send(new DeleteOrderCommand(orderId));

        return NoContent();
    }
}
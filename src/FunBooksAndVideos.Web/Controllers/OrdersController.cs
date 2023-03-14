using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all orders. (This would be accessible only to Admins)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of orders.</returns>
    /// <response code="200">Collection of orders.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("")]
    public async Task<Response<PagedResult<OrderDto>>> GetOrders([FromQuery] GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<OrderDto>>(orders);
    }

    /// <summary>
    /// Create a new order.
    /// </summary>
    /// <param name="request">Order data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    /// <response code="200">Order is placed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpPost]
    [Route("")]
    public async Task<Response<object>> CreateOrder([FromBody] CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return new Response<object>(null);
    }
}

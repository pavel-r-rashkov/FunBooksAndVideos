using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Orders;
using FunBooksAndVideos.Application.Profiles;
using FunBooksAndVideos.Application.Shipments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get shipments for the current user. (This would be accessible only to logged-in user)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of shipments for the current user.</returns>
    /// <response code="200">Collection of shipments for the current user.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("Shipments")]
    public async Task<Response<PagedResult<ShipmentDto>>> GetShipments([FromQuery] GetCustomerShipmentsQuery request, CancellationToken cancellationToken)
    {
        var shipments = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<ShipmentDto>>(shipments);
    }

    /// <summary>
    /// Get orders for the current user. (This would be accessible only to logged-in user)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of orders for the current user.</returns>
    /// <response code="200">Collection of orders for the current user.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("Orders")]
    public async Task<Response<PagedResult<OrderDto>>> GetOrders([FromQuery] GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<OrderDto>>(orders);
    }

    /// <summary>
    /// Get memberships for the current user. (This would be accessible only to logged-in user)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of memberships for the current user.</returns>
    /// <response code="200">Collection of memberships for the current user.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("Memberships")]
    public async Task<Response<PagedResult<MembershipDto>>> GetMemberships([FromRoute] GetMembershipsQuery request, CancellationToken cancellationToken)
    {
        var memberships = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<MembershipDto>>(memberships);
    }
}

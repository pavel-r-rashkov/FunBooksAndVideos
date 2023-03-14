using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Shipments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all shipments. (This would be accessible only to Admins)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of shipments.</returns>
    /// <response code="200">Collection of shipments.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("")]
    public async Task<Response<PagedResult<ShipmentDto>>> GetShipments([FromQuery] GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        var shipments = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<ShipmentDto>>(shipments);
    }
}

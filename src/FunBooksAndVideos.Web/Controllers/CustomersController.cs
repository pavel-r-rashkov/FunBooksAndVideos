using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all customers. (This would be accessible only to Admins)
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of customers.</returns>
    /// <response code="200">Collection of customers.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("")]
    public async Task<Response<PagedResult<CustomerDto>>> GetCustomers([FromQuery] GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<CustomerDto>>(customers);
    }

    /// <summary>
    /// Get customer by ID. (This would be accessible only to Admins)
    /// </summary>
    /// <param name="request">Customer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Data for the requested customer.</returns>
    /// <response code="200">Customer with the specified ID.</response>
    /// <response code="404">If a customer with the specified ID doesn't exist.</response>
    [HttpGet]
    [Route("{CustomerId}")]
    public async Task<Response<CustomerDto>> GetCustomer([FromRoute] GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(request, cancellationToken);
        return new Response<CustomerDto>(customer);
    }
}

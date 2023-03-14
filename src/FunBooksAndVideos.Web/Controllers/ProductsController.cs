using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunBooksAndVideos.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    /// <param name="request">Optional paging, filtering and sorting.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of products.</returns>
    /// <response code="200">Collection of products.</response>
    /// <response code="400">If the specified filter, sort or paging is malformed.</response>
    /// <response code="422">If the input parameters are invalid.</response>
    [HttpGet]
    [Route("")]
    public async Task<Response<PagedResult<ProductDto>>> GetProducts([FromQuery] GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(request, cancellationToken);
        return new Response<PagedResult<ProductDto>>(products);
    }

    /// <summary>
    /// Create a new product. (This would be accessible only to Admins)
    /// </summary>
    /// <param name="request">Product data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    /// <response code="200">Product was created successfully.</response>
    /// <response code="422">If the product data is invalid.</response>
    [HttpPost]
    [Route("")]
    public async Task<Response<object>> CreateProduct([FromBody] CreateProductCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return new Response<object>(null);
    }

    /// <summary>
    /// Get product by ID.
    /// </summary>
    /// <param name="request">Product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Data for the requested product.</returns>
    /// <response code="200">Product with the specified ID.</response>
    /// <response code="404">If a product with the specified ID doesn't exist.</response>
    [HttpGet]
    [Route("{ProductId}")]
    public async Task<Response<ProductDto>> GetProduct([FromRoute] GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(request, cancellationToken);
        return new Response<ProductDto>(product);
    }
}

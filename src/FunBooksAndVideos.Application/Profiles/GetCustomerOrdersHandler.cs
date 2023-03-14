using AutoMapper;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Orders;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerOrdersHandler : IRequestHandler<GetCustomerOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IIdentityProvider _identityProvider;

    public GetCustomerOrdersHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider,
        IIdentityProvider identityProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
        _identityProvider = identityProvider;
    }

    public Task<PagedResult<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var products = _dbContext.GetDbSet<Product>();
        var customers = _dbContext.GetDbSet<Customer>();
        var customerId = _identityProvider.CustomerId();

        return _dbContext
            .GetDbSet<Order>()
            .Where(o => o.CustomerId == customerId)
            .Join(customers, o => o.CustomerId, c => c.Id, (o, c) => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Total = o.Total,
                CustomerFirstName = c.FirstName,
                CustomerLastName = c.LastName,
                OrderLineItems = o.OrderLineItems
                    .Join(
                        products,
                        i => i.ProductQuantity.ProductId,
                        p => p.Id,
                        (i, p) => new OrderLineItemDto
                        {
                            Price = p.Price,
                            Quantity = i.ProductQuantity.Quantity,
                            ProductName = p.Name,
                        }
                    ),
            })
            .Filter(request.Filters)
            .Sort(request.SortOrders)
            .ToPagedResult(request.Pagination, cancellationToken);
    }
}

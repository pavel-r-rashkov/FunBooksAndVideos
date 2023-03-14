using AutoMapper;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using MediatR;

namespace FunBooksAndVideos.Application.Orders;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetOrdersHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public Task<PagedResult<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var products = _dbContext.GetDbSet<Product>();
        var customers = _dbContext.GetDbSet<Customer>();

        return _dbContext
            .GetDbSet<Order>()
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

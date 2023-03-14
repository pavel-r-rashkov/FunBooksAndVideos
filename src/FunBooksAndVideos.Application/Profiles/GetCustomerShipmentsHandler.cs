using AutoMapper;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Shipments;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.Shipping;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerShipmentsHandler : IRequestHandler<GetCustomerShipmentsQuery, PagedResult<ShipmentDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IIdentityProvider _identityProvider;

    public GetCustomerShipmentsHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider,
        IIdentityProvider identityProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
        _identityProvider = identityProvider;
    }

    public Task<PagedResult<ShipmentDto>> Handle(GetCustomerShipmentsQuery request, CancellationToken cancellationToken)
    {
        var products = _dbContext.GetDbSet<Product>();
        var customers = _dbContext.GetDbSet<Customer>();
        var orders = _dbContext.GetDbSet<Order>();
        var customerId = _identityProvider.CustomerId();

        return (
            from s in _dbContext.GetDbSet<ShippingSlip>()
            join o in orders on s.OrderId equals o.Id
            join c in customers on o.CustomerId equals c.Id
            where c.Id == customerId
            select new ShipmentDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                RecipientFirstName = c.FirstName,
                RecipientLastName = c.LastName,
                DeliveryItems = s.DeliveryItems.Join(
                    products,
                    i => i.ProductId,
                    p => p.Id,
                    (i, p) => new DeliveryItemDto
                    {
                        ProductName = p.Name,
                    }),
            })
            .Filter(request.Filters)
            .Sort(request.SortOrders)
            .ToPagedResult(request.Pagination, cancellationToken);
    }
}

using AutoMapper;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.Shipping;
using MediatR;

namespace FunBooksAndVideos.Application.Shipments;

public class GetShipmentsHandler : IRequestHandler<GetShipmentsQuery, PagedResult<ShipmentDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetShipmentsHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public Task<PagedResult<ShipmentDto>> Handle(GetShipmentsQuery request, CancellationToken cancellationToken)
    {
        var orders = _dbContext.GetDbSet<Order>();
        var products = _dbContext.GetDbSet<Product>();
        var customers = _dbContext.GetDbSet<Customer>();

        return (
            from s in _dbContext.GetDbSet<ShippingSlip>()
            join o in orders on s.OrderId equals o.Id
            join c in customers on o.CustomerId equals c.Id
            select new ShipmentDto
            {
                Id = s.Id,
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

using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.Shipping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Shipments;

public class OrderAcceptedHandler : INotificationHandler<OrderAccepted>
{
    private readonly IFunBooksAndVideosContext _dbContext;

    public OrderAcceptedHandler(IFunBooksAndVideosContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(OrderAccepted notification, CancellationToken cancellationToken)
    {
        var productIds = notification.Order.OrderLineItems.Select(i => i.ProductQuantity.ProductId);
        var products = (await _dbContext
            .GetDbSet<Product>()
            .Include(p => p.ProductType)
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken))
            .ToDictionary(p => p.Id);

        var shippingSlip = ShippingSlip.ForOrder(notification.Order, products);

        if (shippingSlip == null)
        {
            // No physical items to deliver
            return;
        }

        await _dbContext.GetDbSet<ShippingSlip>().AddAsync(shippingSlip, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

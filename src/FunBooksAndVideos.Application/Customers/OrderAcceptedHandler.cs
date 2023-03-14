using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Customers;

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

#pragma warning disable CA2016 // False positive
        var customer = await _dbContext
            .GetDbSet<Customer>()
            .FindAsync(notification.Order.CustomerId, cancellationToken);
#pragma warning restore CA2016

        if (customer == null)
        {
            return;
        }

        customer.ActivateMemberships(notification.Order, products);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

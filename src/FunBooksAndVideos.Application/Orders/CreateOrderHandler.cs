using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Orders;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Unit>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IIdentityProvider _identityProvider;

    public CreateOrderHandler(
        IFunBooksAndVideosContext dbContext,
        IIdentityProvider identityProvider)
    {
        _dbContext = dbContext;
        _identityProvider = identityProvider;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var productIds = request.OrderLineItems.Select(i => i.ProductId);
        var products = (await _dbContext
            .GetDbSet<Product>()
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.ProductType)
            .ToListAsync(cancellationToken))
            .ToDictionary(p => p.Id);

        var quantities = request.OrderLineItems.Select(i => new ProductQuantity(i.Quantity, i.ProductId));
        var order = Order.Create(_identityProvider.CustomerId()!.Value, products, quantities);

        await _dbContext.GetDbSet<Order>().AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

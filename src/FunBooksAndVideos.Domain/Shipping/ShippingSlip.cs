using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Shipping;

public class ShippingSlip : Entity
{
    private readonly List<DeliveryItem> _deliveryItems = new();

    private ShippingSlip()
    {
        // ctor for EF
    }

    private ShippingSlip(int orderId, IEnumerable<DeliveryItem> deliveryItems)
    {
        OrderId = orderId;
        _deliveryItems = deliveryItems.ToList();
    }

    public int OrderId { get; private set; }

    public IReadOnlyCollection<DeliveryItem> DeliveryItems => _deliveryItems.AsReadOnly();

    public static ShippingSlip? ForOrder(Order order, IDictionary<int, Product> products)
    {
        var deliveryItems = order.OrderLineItems
            .Where(i => products[i.ProductQuantity.ProductId].ProductType.IsPhysical)
            .Select(i => new DeliveryItem(i.ProductQuantity.ProductId));

        return deliveryItems.Any() ? new ShippingSlip(order.Id, deliveryItems) : null;
    }
}

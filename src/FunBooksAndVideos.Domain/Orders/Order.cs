using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Orders;

public class Order : Entity
{
    private readonly List<OrderLineItem> _orderLineItems = new();

    private Order()
    {
        // ctor for EF
    }

    private Order(int customerId, IEnumerable<OrderLineItem> orderLineItems)
    {
        CustomerId = customerId;
        _orderLineItems = orderLineItems.ToList();
        Total = orderLineItems.Sum(i => i.ProductQuantity.Quantity * i.Price);
        AddEvent(new OrderAccepted(this));
    }

    public int OrderNumber { get; private set; }

    public int CustomerId { get; private set; }

    public decimal Total { get; private set; }

    public IReadOnlyCollection<OrderLineItem> OrderLineItems => _orderLineItems.AsReadOnly();

    public static Order Create(
        int customerId,
        IDictionary<int, Product> products,
        IEnumerable<ProductQuantity> quantities)
    {
        var orderLineItems = quantities.Select(q => new OrderLineItem(q, products[q.ProductId].Price));
        return new Order(customerId, orderLineItems);
    }
}

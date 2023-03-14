using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Orders;

public class OrderLineItem : Entity
{
    private OrderLineItem()
    {
        // ctor for EF
    }

    public OrderLineItem(ProductQuantity productQuantity, decimal price)
    {
        ProductQuantity = productQuantity;
        Price = price;
    }

    public int OrderId { get; private set; }

    public ProductQuantity ProductQuantity { get; set; } = default!;

    public decimal Price { get; private set; }
}

using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Shipping;

public class DeliveryItem : Entity
{
    public DeliveryItem(int productId)
    {
        ProductId = productId;
    }

    public int ShippingSlipId { get; private set; }

    public int ProductId { get; private set; }
}

using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Orders;

public class OrderAccepted : DomainEvent
{
    public OrderAccepted(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
}

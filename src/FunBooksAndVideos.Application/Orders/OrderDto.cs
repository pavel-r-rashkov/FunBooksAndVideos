namespace FunBooksAndVideos.Application.Orders;

public class OrderDto
{
    public int Id { get; set; }

    public int OrderNumber { get; set; }

    public string CustomerFirstName { get; set; } = string.Empty;

    public string CustomerLastName { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public IEnumerable<OrderLineItemDto> OrderLineItems { get; set; } = new List<OrderLineItemDto>();
}

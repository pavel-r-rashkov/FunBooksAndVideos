namespace FunBooksAndVideos.Application.Orders;

public class OrderLineItemDto
{
    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}

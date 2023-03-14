namespace FunBooksAndVideos.Application.Shipments;

public class ShipmentDto
{
    public int Id { get; set; }

    public int OrderNumber { get; set; }

    public string RecipientFirstName { get; set; } = string.Empty;

    public string RecipientLastName { get; set; } = string.Empty;

    public IEnumerable<DeliveryItemDto> DeliveryItems { get; set; } = Array.Empty<DeliveryItemDto>();
}

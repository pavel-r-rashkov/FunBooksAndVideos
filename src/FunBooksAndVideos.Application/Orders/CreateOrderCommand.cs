using MediatR;

namespace FunBooksAndVideos.Application.Orders;

public class CreateOrderCommand : IRequest<Unit>
{
    public IEnumerable<CreateOrderLineItemDto> OrderLineItems { get; set; } = new List<CreateOrderLineItemDto>();

    public class CreateOrderLineItemDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}

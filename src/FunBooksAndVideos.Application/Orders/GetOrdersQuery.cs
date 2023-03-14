using FunBooksAndVideos.Application.Common.CollectionUtils;
using MediatR;

namespace FunBooksAndVideos.Application.Orders;

public class GetOrdersQuery : IRequest<PagedResult<OrderDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(OrderDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

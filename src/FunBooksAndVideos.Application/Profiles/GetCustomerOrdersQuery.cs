using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Orders;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerOrdersQuery : IRequest<PagedResult<OrderDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(OrderDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

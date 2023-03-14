using FunBooksAndVideos.Application.Common.CollectionUtils;
using MediatR;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomersQuery : IRequest<PagedResult<CustomerDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(CustomerDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Application.Shipments;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetCustomerShipmentsQuery : IRequest<PagedResult<ShipmentDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(ShipmentDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

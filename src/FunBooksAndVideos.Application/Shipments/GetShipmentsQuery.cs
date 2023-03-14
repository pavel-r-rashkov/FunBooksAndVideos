using FunBooksAndVideos.Application.Common.CollectionUtils;
using MediatR;

namespace FunBooksAndVideos.Application.Shipments;

public class GetShipmentsQuery : IRequest<PagedResult<ShipmentDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(ShipmentDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

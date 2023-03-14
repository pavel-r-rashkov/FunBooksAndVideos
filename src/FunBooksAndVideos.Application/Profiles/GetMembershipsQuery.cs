using FunBooksAndVideos.Application.Common.CollectionUtils;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetMembershipsQuery : IRequest<PagedResult<MembershipDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(MembershipDto.Name));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

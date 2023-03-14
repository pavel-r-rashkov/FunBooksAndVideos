using FunBooksAndVideos.Application.Common.CollectionUtils;
using MediatR;

namespace FunBooksAndVideos.Application.Products;

public class GetProductsQuery : IRequest<PagedResult<ProductDto>>
{
    public IEnumerable<Filter> Filters { get; set; } = Array.Empty<Filter>();

    public IEnumerable<SortOrder> SortOrders { get; set; } = SortOrder.Default(nameof(ProductDto.Id));

    public Pagination Pagination { get; set; } = Pagination.Default();
}

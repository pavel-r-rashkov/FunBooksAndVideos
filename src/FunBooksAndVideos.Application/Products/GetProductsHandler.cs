using AutoMapper;
using AutoMapper.QueryableExtensions;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Domain.Products;
using MediatR;

namespace FunBooksAndVideos.Application.Products;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetProductsHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return _dbContext
            .GetDbSet<Product>()
            .ProjectTo<ProductDto>(_configurationProvider)
            .Filter(request.Filters)
            .Sort(request.SortOrders)
            .ToPagedResult(request.Pagination, cancellationToken);
    }
}

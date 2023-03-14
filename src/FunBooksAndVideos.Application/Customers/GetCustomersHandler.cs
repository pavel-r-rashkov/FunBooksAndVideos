using AutoMapper;
using AutoMapper.QueryableExtensions;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Domain.Customers;
using MediatR;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomersHandler : IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetCustomersHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        return _dbContext
            .GetDbSet<Customer>()
            .ProjectTo<CustomerDto>(_configurationProvider)
            .Filter(request.Filters)
            .Sort(request.SortOrders)
            .ToPagedResult(request.Pagination, cancellationToken);
    }
}

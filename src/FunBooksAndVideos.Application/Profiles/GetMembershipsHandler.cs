using AutoMapper;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Application.Common.CollectionUtils;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Products;
using MediatR;

namespace FunBooksAndVideos.Application.Profiles;

public class GetMembershipsHandler : IRequestHandler<GetMembershipsQuery, PagedResult<MembershipDto>>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IIdentityProvider _identityProvider;

    public GetMembershipsHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider,
        IIdentityProvider identityProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
        _identityProvider = identityProvider;
    }

    public Task<PagedResult<MembershipDto>> Handle(GetMembershipsQuery request, CancellationToken cancellationToken)
    {
        var products = _dbContext.GetDbSet<Product>();
        var customerId = _identityProvider.CustomerId()!.Value;

        return _dbContext
            .GetDbSet<Customer>()
            .Where(c => c.Id == customerId)
            .SelectMany(c => c.Memberships)
            .Join(products, m => m.ProductId, p => p.Id, (m, p) => new MembershipDto
            {
                ProductId = p.Id,
                Name = p.Name,
            })
            .Filter(request.Filters)
            .Sort(request.SortOrders)
            .ToPagedResult(request.Pagination, cancellationToken);
    }
}

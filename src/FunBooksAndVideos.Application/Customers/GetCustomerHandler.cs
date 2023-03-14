using AutoMapper;
using AutoMapper.QueryableExtensions;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Customers;

public class GetCustomerHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetCustomerHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext
            .GetDbSet<Customer>()
            .ProjectTo<CustomerDto>(_configurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

        return customer ?? throw new ResourceNotFoundException($"Customer with ID: {request.CustomerId} cannot be found");
    }
}

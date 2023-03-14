using AutoMapper;
using AutoMapper.QueryableExtensions;
using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Application.Products;

public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IFunBooksAndVideosContext _dbContext;
    private readonly IConfigurationProvider _configurationProvider;

    public GetProductHandler(
        IFunBooksAndVideosContext dbContext,
        IConfigurationProvider configurationProvider)
    {
        _dbContext = dbContext;
        _configurationProvider = configurationProvider;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext
            .GetDbSet<Product>()
            .ProjectTo<ProductDto>(_configurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.ProductId, cancellationToken);

        return product ?? throw new ResourceNotFoundException($"Product with ID: {request.ProductId} cannot be found");
    }
}

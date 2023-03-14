using FunBooksAndVideos.Application.Common;
using FunBooksAndVideos.Domain.Products;
using MediatR;

namespace FunBooksAndVideos.Application.Products;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly IFunBooksAndVideosContext _dbContext;

    public CreateProductHandler(IFunBooksAndVideosContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(default, request.Name!, request.Price, request.ProductTypeId, null!);
        await _dbContext
            .GetDbSet<Product>()
            .AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

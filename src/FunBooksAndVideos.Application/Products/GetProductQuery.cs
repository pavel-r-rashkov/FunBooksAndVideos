using MediatR;

namespace FunBooksAndVideos.Application.Products;

public class GetProductQuery : IRequest<ProductDto>
{
    public int ProductId { get; set; }
}

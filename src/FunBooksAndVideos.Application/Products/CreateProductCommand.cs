using MediatR;

namespace FunBooksAndVideos.Application.Products;

public class CreateProductCommand : IRequest<Unit>
{
    public string? Name { get; set; }

    public decimal Price { get; set; }

    public int ProductTypeId { get; set; }
}

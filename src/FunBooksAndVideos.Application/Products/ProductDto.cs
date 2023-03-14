namespace FunBooksAndVideos.Application.Products;

public class ProductDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public int ProductTypeId { get; set; }

    public string? ProductTypeName { get; set; }
}

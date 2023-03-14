using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Products;

public class ProductQuantity : ValueObject
{
    private ProductQuantity()
    {
    }

    public ProductQuantity(int quantity, int productId)
    {
        Quantity = quantity;
        ProductId = productId;
    }

    public int Quantity { get; }

    public int ProductId { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Quantity;
        yield return ProductId;
    }
}

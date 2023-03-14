using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Customers;

public class Membership : ValueObject
{
    private Membership()
    {
    }

    public Membership(int customerId, int productId)
    {
        CustomerId = customerId;
        ProductId = productId;
    }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CustomerId;
        yield return ProductId;
    }
}

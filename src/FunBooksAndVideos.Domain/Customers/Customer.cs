using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.SeedWork;

namespace FunBooksAndVideos.Domain.Customers;

public class Customer : Entity
{
    private readonly List<Membership> _memberships = new();

    public Customer(int id)
    {
        Id = id;
    }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public virtual IReadOnlyCollection<Membership> Memberships => _memberships.AsReadOnly();

    public void ActivateMemberships(Order order, IDictionary<int, Product> products)
    {
        if (order.CustomerId != Id)
        {
            throw new ArgumentException($"Customer {Id} cannot activate order {order.Id} made by customer {order.CustomerId}");
        }

        var newMemberships = order.OrderLineItems
            .Where(i => products[i.ProductQuantity.ProductId].ProductType.RequiresActivation)
            .Select(i => new Membership(Id, i.ProductQuantity.ProductId));

        foreach (var newMembership in newMemberships)
        {
            if (_memberships.Contains(newMembership))
            {
                // Membership already exists - skip. The requirements don't specify what needs to happen in this case.
                // We could do something like renewing the membership.
                continue;
            }

            _memberships.Add(newMembership);
        }
    }
}

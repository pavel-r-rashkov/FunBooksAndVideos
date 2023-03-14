using System.Collections.Generic;
using System.Linq;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using Xunit;

namespace FunBooksAndVideos.Domain.UnitTests;

public class OrderTests
{
    [Fact]
    public void Create_ShouldCreateOrderLineItems()
    {
        var (products, quantities) = CreateTestData();

        var order = Order.Create(1, products, quantities);

        Assert.Equal(quantities.Count(), order.OrderLineItems.Count);
    }

    [Fact]
    public void Create_ShouldEmitOrderAcceptedEvent()
    {
        var (products, quantities) = CreateTestData();

        var order = Order.Create(1, products, quantities);

        Assert.Single(order.Events);
        var @event = order.Events.First();
        Assert.IsType<OrderAccepted>(@event);
    }

    [Fact]
    public void Create_ShouldCalculateTotalPrice()
    {
        var (products, quantities) = CreateTestData();

        var order = Order.Create(1, products, quantities);

        Assert.Equal(77.0m, order.Total);
    }

    private static (IDictionary<int, Product>, IEnumerable<ProductQuantity>) CreateTestData()
    {
        var products = new Dictionary<int, Product>
        {
            { 1, new Product(1, "ProductA", 10.0m, 1, new ProductType(1, "TypeA", false, false)) },
            { 2, new Product(2, "ProductA", 15.0m, 2, new ProductType(2, "TypeB", true, false)) },
            { 3, new Product(3, "ProductA", 12.0m, 3, new ProductType(3, "TypeC", false, true)) },
        };
        var quantities = new[]
        {
            new ProductQuantity(2, 1),
            new ProductQuantity(3, 2),
            new ProductQuantity(1, 3),
        };

        return (products, quantities);
    }
}

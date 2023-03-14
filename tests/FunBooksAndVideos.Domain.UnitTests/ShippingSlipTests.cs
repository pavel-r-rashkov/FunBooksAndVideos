using System.Collections.Generic;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using FunBooksAndVideos.Domain.Shipping;
using Xunit;

namespace FunBooksAndVideos.Domain.UnitTests;

public class ShippingSlipTests
{
    [Fact]
    public void ForOrder_ShouldCreateShippingSlipWithDeliveryItems()
    {
        var (order, products) = CreateOrder(true, true);

        var shippingSlip = ShippingSlip.ForOrder(order, products);

        Assert.NotNull(shippingSlip);
        Assert.Equal(shippingSlip.DeliveryItems.Count, products.Keys.Count);
    }

    [Fact]
    public void ForOrder_WithNonPhysicalItems_ShouldSkipNonPhysicalItems()
    {
        var (order, products) = CreateOrder(true, false);

        var shippingSlip = ShippingSlip.ForOrder(order, products);

        Assert.NotNull(shippingSlip);
        Assert.Single(shippingSlip.DeliveryItems);
    }

    [Fact]
    public void ForOrder_WithNoPhysicalItems_ShouldReturnNull()
    {
        var (order, products) = CreateOrder(false, false);

        var shippingSlip = ShippingSlip.ForOrder(order, products);

        Assert.Null(shippingSlip);
    }

    private static (Order, IDictionary<int, Product>) CreateOrder(bool physicalA, bool physicalB)
    {
        var productTypeA = new ProductType(1, "TypeA", physicalA, false);
        var productTypeB = new ProductType(2, "TypeB", physicalB, false);
        var products = new Dictionary<int, Product>
        {
            { 1, new Product(1, "ProductA", 10.0m, productTypeA.Id, productTypeA) },
            { 2, new Product(2, "ProductB", 20.0m, productTypeB.Id, productTypeB) },
        };

        var order = Order.Create(
            1,
            products,
            new[]
            {
                new ProductQuantity(1, products[1].Id),
                new ProductQuantity(1, products[2].Id),
            });

        return (order, products);
    }
}

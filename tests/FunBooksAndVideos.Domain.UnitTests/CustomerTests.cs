using System;
using System.Collections.Generic;
using System.Linq;
using FunBooksAndVideos.Domain.Customers;
using FunBooksAndVideos.Domain.Orders;
using FunBooksAndVideos.Domain.Products;
using Xunit;

namespace FunBooksAndVideos.Domain.UnitTests;

public class CustomerTests
{
    [Fact]
    public void ActivateMemberships_WithForeignOrder_ShouldThrowException()
    {
        var customer = new Customer(1);
        var ownerId = 99;

        var order = Order.Create(ownerId, new Dictionary<int, Product>(), Array.Empty<ProductQuantity>());

        Assert.Throws<ArgumentException>(() => customer.ActivateMemberships(order, new Dictionary<int, Product>()));
    }

    [Fact]
    public void ActivateMemberships_WithActivableProducts_ShouldAddActivableProducts()
    {
        var customer = new Customer(1);
        var (order, products) = CreateOrder(1, true);

        customer.ActivateMemberships(order, products);

        Assert.Equal(2, customer.Memberships.Count);
        Assert.Equivalent(products.Keys, customer.Memberships.Select(m => m.ProductId));
    }

    [Fact]
    public void ActivateMemberships_WithNonActivableProducts_ShouldSkipNonActivableProducts()
    {
        var customer = new Customer(1);
        var (order, products) = CreateOrder(1, false);

        customer.ActivateMemberships(order, products);

        Assert.Empty(customer.Memberships);
    }

    [Fact]
    public void ActivateMemberships_WithExistingMembership_ShouldNotAddDuplicateMembership()
    {
        var customer = new Customer(1);
        var (order, products) = CreateOrder(customer.Id, true);
        customer.ActivateMemberships(order, products);
        var (newOrder, newProducts) = CreateOrder(customer.Id, true);

        customer.ActivateMemberships(newOrder, newProducts);

        Assert.Equal(products.Count, customer.Memberships.Count);
    }

    private static (Order, IDictionary<int, Product>) CreateOrder(int orderId, bool activable)
    {
        var productType = new ProductType(1, "TypeA", false, activable);
        var products = new Dictionary<int, Product>
        {
            { 1, new Product(1, "ProductA", 10.0m, productType.Id, productType) },
            { 2, new Product(2, "ProductB", 20.0m, productType.Id, productType) },
        };

        var order = Order.Create(
            orderId,
            products,
            new[]
            {
                new ProductQuantity(1, products[1].Id),
                new ProductQuantity(1, products[2].Id),
            });

        return (order, products);
    }
}

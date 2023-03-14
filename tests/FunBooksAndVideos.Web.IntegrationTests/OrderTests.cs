using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunBooksAndVideos.Application.Orders;
using Xunit;
using static FunBooksAndVideos.Application.Orders.CreateOrderCommand;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class OrderTests : BaseTest
{
    public OrderTests(TestWebApplicationFactory appFactory)
        : base(appFactory)
    {
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=orderNumber dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=orderNumber foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetOrders_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/orders?{collectionParams}");

        Assert.False(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("filter=orderNumber eq 1122334")]
    [InlineData("filter=orderNumber eq 1122334, id gt 3")]
    [InlineData("orderBy=id")]
    [InlineData("orderBy=id desc")]
    [InlineData("orderBy=orderNumber asc, id desc")]
    [InlineData("skip=5")]
    [InlineData("take=10")]
    [InlineData("filter=id gt 3&take=10&skip=5&orderBy=orderNumber desc")]
    public async Task GetOrders_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/orders?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [MemberData(nameof(ValidOrders))]
    public async Task PostOrder_WithValidData_ShouldReturnSuccess(CreateOrderCommand order)
    {
        using var contents = new JsonHttpContent(order);

        var response = await this.Client.PostAsync("/orders", contents);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [MemberData(nameof(InvalidOrders))]
    public async Task PostOrder_WithInvalidData_ShouldReturnError(CreateOrderCommand order)
    {
        using var contents = new JsonHttpContent(order);

        var response = await this.Client.PostAsync("/orders", contents);

        Assert.False(response.IsSuccessStatusCode);
    }

    public static IEnumerable<object[]> ValidOrders()
    {
        yield return new object[]
        {
            new CreateOrderCommand
            {
                OrderLineItems = new []
                {
                    new CreateOrderLineItemDto
                    {
                        ProductId = 1,
                        Quantity = 10,
                    }
                },
            },
        };

        yield return new object[]
        {
            new CreateOrderCommand
            {
                OrderLineItems = new []
                {
                    new CreateOrderLineItemDto
                    {
                        ProductId = 1,
                        Quantity = 10,
                    },
                    new CreateOrderLineItemDto
                    {
                        ProductId = 2,
                        Quantity = 5,
                    }
                },
            },
        };
    }

    public static IEnumerable<object[]> InvalidOrders()
    {
        yield return new object[]
        {
            new CreateOrderCommand
            {
                OrderLineItems = Array.Empty<CreateOrderLineItemDto>(),
            },
        };

        yield return new object[]
        {
            new CreateOrderCommand
            {
                OrderLineItems = new []
                {
                    new CreateOrderLineItemDto
                    {
                        ProductId = 0,
                        Quantity = 10,
                    }
                },
            },
        };

        yield return new object[]
        {
            new CreateOrderCommand
            {
                OrderLineItems = new []
                {
                    new CreateOrderLineItemDto
                    {
                        ProductId = 1,
                        Quantity = 0,
                    }
                },
            },
        };
    }
}

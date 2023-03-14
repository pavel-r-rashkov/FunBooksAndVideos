using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FunBooksAndVideos.Application.Products;
using Xunit;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class ProductTests : BaseTest
{
    public ProductTests(TestWebApplicationFactory appFactory)
        : base(appFactory)
    {
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=name dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=name foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetProducts_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/products?{collectionParams}");

        Assert.False(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("filter=name eq 'ProductA'")]
    [InlineData("filter=name eq 'ProductA', id gt 3")]
    [InlineData("orderBy=id")]
    [InlineData("orderBy=id desc")]
    [InlineData("orderBy=name asc, id desc")]
    [InlineData("skip=5")]
    [InlineData("take=10")]
    [InlineData("filter=id gt 3&take=10&skip=5&orderBy=name desc")]
    public async Task GetProducts_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/products?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetProduct_WithExistingProduct_ShouldReturn200Ok()
    {
        var product = Context.Products.First();
        var response = await Client.GetAsync($"/products/{product.Id}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetProduct_WithNonExistingProduct_ShouldReturnNotFound()
    {
        var productId = Context.Products.Max(c => c.Id) + 1;
        var response = await Client.GetAsync($"/products/{productId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(ValidProducts))]
    public async Task PostProduct_WithValidData_ShouldReturnSuccess(CreateProductCommand product)
    {
        using var contents = new JsonHttpContent(product);

        var response = await this.Client.PostAsync("/products", contents);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [MemberData(nameof(InvalidProducts))]
    public async Task PostProduct_WithInvalidData_ShouldReturnError(CreateProductCommand product)
    {
        using var contents = new JsonHttpContent(product);

        var response = await this.Client.PostAsync("/products", contents);

        Assert.False(response.IsSuccessStatusCode);
    }

    public static IEnumerable<object[]> ValidProducts()
    {
        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "Product A",
                Price = 10.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "Product B",
                Price = 9999.99m,
                ProductTypeId = 1,
            },
        };
    }

    public static IEnumerable<object[]> InvalidProducts()
    {
        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "",
                Price = 10.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = new string('a', 513),
                Price = 10.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = null,
                Price = 10.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "Product A",
                Price = 0.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "Product A",
                Price = -10.0m,
                ProductTypeId = 1,
            },
        };

        yield return new object[]
        {
            new CreateProductCommand
            {
                Name = "Product A",
                Price = 10.0m,
                ProductTypeId = 0,
            },
        };
    }
}

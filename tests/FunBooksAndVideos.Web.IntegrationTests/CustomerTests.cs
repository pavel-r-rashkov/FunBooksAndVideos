using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class CustomerTests : BaseTest
{
    public CustomerTests(TestWebApplicationFactory appFactory)
        : base(appFactory)
    {
    }

    [Fact]
    public async Task GetCustomer_WithExistingCustomer_ShouldReturn200Ok()
    {
        var customer = Context.Customers.First();
        var response = await Client.GetAsync($"/customers/{customer.Id}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetCustomer_WithNonExistingCustomer_ShouldReturnNotFound()
    {
        var customerId = Context.Customers.Max(c => c.Id) + 1;
        var response = await Client.GetAsync($"/customers/{customerId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("filter=missingProperty eq 'Test'")]
    [InlineData("filter=firstName dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=firstName foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetCustomers_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/customers?{collectionParams}");

        Assert.False(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("filter=firstName eq 'Test'")]
    [InlineData("filter=firstName eq 'Test', id gt 3")]
    [InlineData("orderBy=id")]
    [InlineData("orderBy=id desc")]
    [InlineData("orderBy=firstName asc, id desc")]
    [InlineData("skip=5")]
    [InlineData("take=10")]
    [InlineData("filter=id gt 3&take=10&skip=5&orderBy=id desc")]
    public async Task GetCustomers_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/customers?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }
}

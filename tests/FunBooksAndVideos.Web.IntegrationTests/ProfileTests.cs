using System.Threading.Tasks;
using Xunit;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class ProfileTests : BaseTest
{
    public ProfileTests(TestWebApplicationFactory appFactory)
        : base(appFactory)
    {
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=name dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=orderNumber foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetCustomerOrders_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/orders?{collectionParams}");

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
    public async Task GetCustomerOrders_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/orders?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=recipientFirstName dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=orderNumber foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetCustomerShipments_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/shipments?{collectionParams}");

        Assert.False(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("filter=recipientFirstName eq 'Test'")]
    [InlineData("filter=recipientFirstName eq 'Test', id gt 3")]
    [InlineData("orderBy=id")]
    [InlineData("orderBy=id desc")]
    [InlineData("orderBy=orderNumber asc, id desc")]
    [InlineData("skip=5")]
    [InlineData("take=10")]
    [InlineData("filter=id gt 3&take=10&skip=5&orderBy=orderNumber desc")]
    public async Task GetCustomerShipments_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/shipments?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=name dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=name foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetCustomerMemberships_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/memberships?{collectionParams}");

        Assert.False(response.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("filter=name eq 'Test'")]
    [InlineData("filter=name eq 'Test', productId gt 3")]
    [InlineData("orderBy=name")]
    [InlineData("orderBy=name desc")]
    [InlineData("orderBy=name asc, productId desc")]
    [InlineData("skip=5")]
    [InlineData("take=10")]
    [InlineData("filter=productId gt 3&take=10&skip=5&orderBy=name desc")]
    public async Task GetCustomerMemberships_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/profile/memberships?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }
}

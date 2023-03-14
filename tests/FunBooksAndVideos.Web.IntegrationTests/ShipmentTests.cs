using System.Threading.Tasks;
using Xunit;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class ShipmentTests : BaseTest
{
    public ShipmentTests(TestWebApplicationFactory appFactory)
        : base(appFactory)
    {
    }

    [Theory]
    [InlineData("filter=missingProperty eq 123")]
    [InlineData("filter=recipientFirstName dd 'Test'")]
    [InlineData("orderBy=missingProperty")]
    [InlineData("orderBy=missingProperty asc")]
    [InlineData("orderBy=orderNumber foo")]
    [InlineData("skip=-1")]
    [InlineData("take=-1")]
    public async Task GetShipments_WithInvalidCollectionParams_ShouldReturnError(string collectionParams)
    {
        var response = await Client.GetAsync($"/shipments?{collectionParams}");

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
    public async Task GetShipments_WithValidCollectionParams_ShouldReturnSuccess(string collectionParams)
    {
        var response = await Client.GetAsync($"/shipments?{collectionParams}");

        Assert.True(response.IsSuccessStatusCode);
    }
}

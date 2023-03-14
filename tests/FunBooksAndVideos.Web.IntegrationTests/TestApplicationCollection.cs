using Xunit;

namespace FunBooksAndVideos.Web.IntegrationTests;

[CollectionDefinition(Constants.TestCollection)]
public class TestApplicationCollectionDefinition : ICollectionFixture<TestWebApplicationFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition]
}

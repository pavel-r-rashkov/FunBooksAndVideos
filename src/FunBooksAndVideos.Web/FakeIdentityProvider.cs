using FunBooksAndVideos.Application.Common;

namespace FunBooksAndVideos.Web;

/// <summary>
/// FakeIdentityProvider returns a hardcoded customer ID from appsetting.json in order to simulate a logged in user.
/// </summary>
public class FakeIdentityProvider : IIdentityProvider
{
    private const string CUSTOMER_CONFIG_KEY = "FakeCustomerId";
    private readonly IConfiguration _configuration;

    public FakeIdentityProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public int? CustomerId()
    {
        return _configuration.GetValue<int?>(CUSTOMER_CONFIG_KEY);
    }
}

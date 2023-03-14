using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FunBooksAndVideos.Web.IntegrationTests;

public class JsonHttpContent : StringContent
{
    public JsonHttpContent(object contents)
        : base(JsonSerializer.Serialize(contents), Encoding.UTF8, "application/json")
    {
    }
}

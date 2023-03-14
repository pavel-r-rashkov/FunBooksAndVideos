using FunBooksAndVideos.Web;

var builder = WebApplication.CreateBuilder(args)
    .AddAppConfig()
    .AddLogging()
    .AddControllers()
    .AddAutoMapper()
    .AddValidators()
    .AddIdentity()
    .AddSwagger()
    .AddMediatR()
    .AddDbContext();

var app = builder.Build();
app.MapControllers();
app
    .UseCustomSwagger()
    .UseHttpLogging()
    .UseHttpsRedirection()
    .UseAuthorization();

app.MigrateAndRun();

// Allows test project to reference Program
namespace FunBooksAndVideos.Web
{
    public partial class Program { }
}

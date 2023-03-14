using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideos.Database;

public class Program
{
    public static void Main()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("FunBooksAndVideosConnectionString");

        PerformUpgrade(connectionString, null);
    }

    public static void PerformUpgrade(string connectionString, ILogger? logger)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var builder = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly());

        builder = logger != null
            ? builder.LogTo(new DbUpgradeLogger(logger))
            : builder.LogToConsole();

        var upgrader = builder.Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new DbUpgradeException(result.Error.ToString());
        }
    }
}

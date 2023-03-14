using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;

namespace FunBooksAndVideos.Database;

internal sealed class DbUpgradeLogger : IUpgradeLog
{
    private readonly ILogger _logger;

    public DbUpgradeLogger(ILogger logger)
    {
        _logger = logger;
    }

#pragma warning disable CA1848
#pragma warning disable CA2254

    public void WriteError(string format, params object[] args)
    {
        _logger.LogError(format, args);
    }

    public void WriteInformation(string format, params object[] args)
    {
        _logger.LogInformation(format, args);
    }

    public void WriteWarning(string format, params object[] args)
    {
        _logger.LogWarning(format, args);
    }

#pragma warning restore CA1848
#pragma warning restore CA2254

}

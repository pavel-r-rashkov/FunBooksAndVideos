namespace FunBooksAndVideos.Database;

public class DbUpgradeException : Exception
{
    public DbUpgradeException(string message)
        : base(message)
    {
    }

    public DbUpgradeException(string message, Exception ex)
        : base(message, ex)
    {
    }
}

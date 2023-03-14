namespace FunBooksAndVideos.Application.Common;

public class AppConfig
{
    public ConnectionStringsSection? ConnectionStrings { get; set; }

    public bool SkipDatabaseMigrations { get; set; }

    public class ConnectionStringsSection
    {
        public string? FunBooksAndVideosConnectionString { get; set; }
    }
}

namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class Pagination
{
    public const int DefaultSkip = 0;
    public const int DefaultTake = 10;

    public Pagination(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    public int Skip { get; set; }

    public int Take { get; set; }

    public static Pagination Default()
    {
        return new Pagination(DefaultSkip, DefaultTake);
    }
}

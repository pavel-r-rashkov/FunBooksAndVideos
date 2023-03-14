namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class PagedResult<T>
{
    public PagedResult()
    {
        Items = Array.Empty<T>();
    }

    public PagedResult(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; set; }
}

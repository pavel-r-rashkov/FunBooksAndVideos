namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class SortOrder
{
    public string PropertyName { get; set; } = null!;

    public SortDirection Direction { get; set; }

    public static IEnumerable<SortOrder> Default(string propertyName)
    {
        yield return new SortOrder
        {
            PropertyName = propertyName,
            Direction = SortDirection.Ascending,
        };
    }
}

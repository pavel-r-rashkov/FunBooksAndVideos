namespace FunBooksAndVideos.Application.Common.CollectionUtils;

public class Filter
{
    public string PropertyName { get; set; } = null!;

    public FilterOperator Operator { get; set; }

    public string? Value { get; set; } = null!;
}

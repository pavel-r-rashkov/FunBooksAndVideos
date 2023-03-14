namespace FunBooksAndVideos.Web.CollectionUtils;

public class CollectionBindingException : Exception
{
    public CollectionBindingException(string message)
        : base(message)
    {
    }

    public CollectionBindingException(string message, Exception ex)
        : base(message, ex)
    {
    }
}

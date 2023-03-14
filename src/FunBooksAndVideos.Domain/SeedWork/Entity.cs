namespace FunBooksAndVideos.Domain.SeedWork;

public abstract class Entity
{
    private readonly List<DomainEvent> _events = new();

    public int Id { get; protected set; }

    public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void AddEvent(DomainEvent e)
    {
        _events.Add(e);
    }
}

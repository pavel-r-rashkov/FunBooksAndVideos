using MediatR;

namespace FunBooksAndVideos.Domain.SeedWork;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        OccuredOn = DateTime.Now;
    }

    public DateTime OccuredOn { get; private set; }
}

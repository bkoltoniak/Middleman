using System.Threading.Tasks;

namespace Middleman.Interfaces
{
    /// <summary>
    /// Represents an event that can be handled by zero or more <see cref="IEventHandler{TEvent}"/> implementations.
    /// </summary>
    public interface IEvent
    {
    }

    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent command);
    }
}

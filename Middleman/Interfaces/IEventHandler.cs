using System.Threading.Tasks;

namespace Middleman.Interfaces
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent command);
    }
}

using System.Threading.Tasks;

namespace Middleman.Interfaces
{
    public interface IDispatcher : IRequestDispatcher, IEventDispatcher
    {
    }

    public interface IRequestDispatcher
    {
        Task Dispatch(IRequest request);

        Task<TResponse> Dispatch<TResponse>(IRequest<TResponse> request);
    }

    public interface IEventDispatcher
    {
        Task Dispatch(IEvent @event);
    }
}

using System.Threading.Tasks;

namespace Middleman.Interfaces
{

    public delegate Task<object?> DispatcherDelegate(object message);

    public interface IDispatcherMiddleware
    {
        Task<object?> Handle(object message, DispatcherDelegate next);
    }
}

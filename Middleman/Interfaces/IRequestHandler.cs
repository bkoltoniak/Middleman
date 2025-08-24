using System.Threading.Tasks;

namespace Middleman.Interfaces
{
    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest query);
    }

    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest query);
    }
}

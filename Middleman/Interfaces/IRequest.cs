using System.Threading.Tasks;

namespace Middleman.Interfaces
{
    /// <summary>
    /// Base request interface.
    /// </summary>
    public interface IRequestBase
    {
    }

    /// <summary>
    /// Represents a request that will be handled by single <see cref="IRequestHandler{TRequest}"/> implementation.
    /// </summary>
    public interface IRequest : IRequestBase
    {
    }

    /// <summary>
    /// Represents a request that will be handled by single <see cref="IRequestHandler{TRequest, TResponse}"/> implementation.
    /// </summary>
    public interface IRequest<TResponse> : IRequestBase
    {
    }

    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest query);
    }

    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest query);
    }
}

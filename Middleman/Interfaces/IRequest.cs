namespace Middleman.Interfaces
{
    /// <summary>
    /// Represents a request that will be handled by single <see cref="IRequestHandler{TRequest}"/> implementation.
    /// </summary>
    public interface IRequest
    {
    }

    /// <summary>
    /// Represents a request that will be handled by single <see cref="IRequestHandler{TRequest, TResponse}"/> implementation.
    /// </summary>
    public interface IRequest<TResponse>
    {
    }
}

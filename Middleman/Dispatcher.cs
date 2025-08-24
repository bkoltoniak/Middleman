using Middleman.Exceptions;
using Middleman.Interfaces;
using Middleman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleman
{
    /// <summary>
    /// Represents a dispatcher that can dispatch requests and events to their respective handlers.
    /// It is a implementation of the mediator pattern.
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _services;

        public Dispatcher(IServiceProvider services)
        {
            _services = services;
        }

        /// <summary>
        /// Dispatches the <paramref name="request"/> to be handled by single <see cref="IRequestHandler{TRequest}"/> registered in the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="request">The message to dispatch.</param>
        /// <returns>A task representing work of the <see cref="IRequestHandler{TRequest}"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HandlerNotFoundException"></exception>
        /// <exception cref="DispatcherException"></exception>
        public async Task Dispatch(IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Type handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

            await WrapWithMiddleware(MessageDescriptor.Request(request.GetType()),
                async message =>
                {
                    await ((Task)InvokeHandle(handlerType, GetRequestHandlerOrThrow(handlerType), message));
                    return null;
                })
            (request);
        }


        /// <summary>
        /// Dispatches the <paramref name="request"/> to be handled by single <see cref="IRequestHandler{TRequest, TResponse}"/> registered in the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="request">The message to dispatch.</param>
        /// <returns>A task representing work of the <see cref="IRequestHandler{TRequest, TResponse}"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HandlerNotFoundException"></exception>
        /// <exception cref="DispatcherException"></exception>
        public async Task<TResponse> Dispatch<TResponse>(IRequest<TResponse> request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            return (TResponse)(await WrapWithMiddleware(MessageDescriptor.RequestWithResponse(request.GetType()),
                async message =>
                {
                    TResponse response = await ((Task<TResponse>)InvokeHandle(handlerType, GetRequestHandlerOrThrow(handlerType), message));
                    return response;
                })
            (request))!;
        }


        /// <summary>
        /// Dispatches the <paramref name="event"/> that can be handled by zero or many <see cref="IEventHandler{TEvent}"/> registered in the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="event">The event to dispatch.</param>
        /// <returns>A task representing work of the all the <see cref="IEventHandler{TEvent}"/> that handle the event.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="HandlerNotFoundException"></exception>
        /// <exception cref="DispatcherException"></exception>
        public async Task Dispatch(IEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            Type handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            await WrapWithMiddleware(MessageDescriptor.EventDescriptor(@event.GetType()),
                async message =>
                {
                    await Task.WhenAll(GetEventHandlers(handlerType).Select(x => (Task)InvokeHandle(handlerType, x, message)));
                    return null;
                })
            (@event);
        }


        private object InvokeHandle(Type handlerType, object handler, object parameter)
        {
            var method = handlerType.GetMethod("Handle", new[] { parameter.GetType() });
            if (method == null)
                throw new DispatcherException($"Could not resolve a Handle({parameter.GetType().Name}) method in the handler type.");

            return method.Invoke(handler, new object[] { parameter })!;
        }

        private IEnumerable<object> GetEventHandlers(Type handlerType)
        {
            Type handlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            IEnumerable<object>? handlers = (IEnumerable<object>?)_services.GetService(handlersType);
            if (handlers != null) return handlers;

            return Enumerable.Empty<object>();
        }

        private object GetRequestHandlerOrThrow(Type handlerType)
        {
            object? handler = _services.GetService(handlerType);
            if (handler == null) throw new HandlerNotFoundException(handlerType);

            return handler;
        }

        private DispatcherDelegate WrapWithMiddleware(MessageDescriptor messageDescriptor, DispatcherDelegate handleDelegate)
        {
            foreach (IDispatcherMiddleware middleware in GetMiddleware())
            {
                DispatcherDelegate next = handleDelegate;
                handleDelegate = request => middleware.Handle(request, messageDescriptor, next);
            }

            return handleDelegate;
        }

        private IEnumerable<IDispatcherMiddleware> GetMiddleware()
        {
            Type handlersType = typeof(IEnumerable<>).MakeGenericType(typeof(IDispatcherMiddleware));
            IEnumerable<IDispatcherMiddleware>? middleware = (IEnumerable<IDispatcherMiddleware>?)_services.GetService(handlersType);
            if (middleware != null) return middleware.Reverse();

            return Enumerable.Empty<IDispatcherMiddleware>();
        }
    }
}

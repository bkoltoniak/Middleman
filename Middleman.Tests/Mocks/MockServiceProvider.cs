using Middleman.Interfaces;
using Moq;
using System;
using System.Collections.Generic;

namespace Middleman.Tests.Mocks
{
    internal class MockServiceProvider : Mock<IServiceProvider>, IServiceProvider
    {
        public void SetupEventHandlers<TEvent>(params IEventHandler<TEvent>[] events) where TEvent : IEvent
        {
            Setup(x => x.GetService(typeof(IEnumerable<IEventHandler<TEvent>>))).Returns(events);
        }

        public void SetupRequestHandler<TRequest>(IRequestHandler<TRequest> handler) where TRequest : IRequest
        {
            Setup(x => x.GetService(typeof(IRequestHandler<TRequest>))).Returns(handler);

        }

        public void SetupRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>
        {
            Setup(x => x.GetService(typeof(IRequestHandler<TRequest, TResponse>))).Returns(handler);
        }

        public object? GetService(Type serviceType)
        {
            return Object.GetService(serviceType);
        }
    }
}

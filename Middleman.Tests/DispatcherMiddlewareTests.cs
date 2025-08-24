
using Middleman.Interfaces;
using Middleman.Tests.Mocks;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    public class DispatcherMiddlewareTests
    {
        [Fact]
        public async Task Dispatch_WhenHandlingRequest_AndTwoMiddlewareRegistered_AndHandlerRegistered_ShouldInvokeMiddlewareAndHandlerInOrder()
        {
            // Arrange.
            List<string> log = new List<string>();
            MockServiceProvider services = new MockServiceProvider();
            MockRequestHandler handler = new MockRequestHandler("Handler", log);
            MockDispatcherMiddleware middleware = new MockDispatcherMiddleware("Middleware 1", log);
            MockDispatcherMiddleware middleware2 = new MockDispatcherMiddleware("Middleware 2", log);
            services.SetupMiddleware(middleware, middleware2);
            services.SetupRequestHandler(handler);
            IRequestDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeRequest());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeRequest>()), Times.Once);
            middleware.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            middleware2.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            Assert.Equal(
                new string[] { "Middleware 1", "Middleware 2", "Handler", "Middleware 2", "Middleware 1" },
                log);
        }

        [Fact]
        public async Task Dispatch_WhenHandlingResponseRequest_AndTwoMiddlewareRegistered_AndHandlerRegistered_ShouldInvokeMiddlewareAndHandlerInOrder()
        {
            // Arrange.
            List<string> log = new List<string>();
            MockServiceProvider services = new MockServiceProvider();
            MockResponseRequestHandler handler = new MockResponseRequestHandler("Handler", log);
            MockDispatcherMiddleware middleware = new MockDispatcherMiddleware("Middleware 1", log);
            MockDispatcherMiddleware middleware2 = new MockDispatcherMiddleware("Middleware 2", log);
            services.SetupMiddleware(middleware, middleware2);
            services.SetupRequestHandler(handler);
            IRequestDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeResponseRequest());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeResponseRequest>()), Times.Once);
            middleware.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            middleware2.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            Assert.Equal(
                new string[] { "Middleware 1", "Middleware 2", "Handler", "Middleware 2", "Middleware 1" },
                log);
        }

        [Fact]
        public async Task Dispatch_WhenHandlingEventRequest_AndTwoMiddlewareRegistered_AndHandlerRegistered_ShouldInvokeMiddlewareAndHandlerInOrder()
        {
            // Arrange.
            List<string> log = new List<string>();
            MockServiceProvider services = new MockServiceProvider();
            MockEventHandler handler = new MockEventHandler("Handler", log);
            MockDispatcherMiddleware middleware = new MockDispatcherMiddleware("Middleware 1", log);
            MockDispatcherMiddleware middleware2 = new MockDispatcherMiddleware("Middleware 2", log);
            services.SetupMiddleware(middleware, middleware2);
            services.SetupEventHandlers(handler);
            IEventDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeEvent());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeEvent>()), Times.Once);
            middleware.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            middleware2.Verify(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>()), Times.Once);
            Assert.Equal(
                new string[] { "Middleware 1", "Middleware 2", "Handler", "Middleware 2", "Middleware 1" },
                log);
        }
    }
}


using Middleman.Interfaces;
using Middleman.Tests.Mocks;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    public class DispatcherMiddlewareTests2
    {
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
    }
}

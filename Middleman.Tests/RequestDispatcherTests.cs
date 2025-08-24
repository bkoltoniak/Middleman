using Middleman.Exceptions;
using Middleman.Interfaces;
using Middleman.Tests.Mocks;
using Moq;
using System;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    public class RequestDispatcherTests
    {
        [Fact]
        public async Task Dispatch_WhenRequestAndNoHandlerRegistered_Throws()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            Dispatcher sut = new Dispatcher(services);

            // Act.
            Func<Task> act = () => sut.Dispatch(new FakeRequest());

            // Assert.
            await Assert.ThrowsAsync<HandlerNotFoundException>(act);
            services.Verify(x => x.GetService(typeof(IRequestHandler<FakeRequest>)), Times.Once);
        }

        [Fact]
        public async Task Dispatch_WhenResponseRequestAndNoHandlerRegistered_Throws()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            IRequestDispatcher sut = new Dispatcher(services);

            // Act.
            Func<Task> act = () => sut.Dispatch(new FakeResponseRequest());

            // Assert.
            await Assert.ThrowsAsync<HandlerNotFoundException>(act);
            services.Verify(x => x.GetService(typeof(IRequestHandler<FakeResponseRequest, FakeResponse>)), Times.Once);
        }

        [Fact]
        public async Task Dispatch_WhenRequest_RequestHandlerIsCalledOnce()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            MockRequestHandler handler = new MockRequestHandler();
            services.SetupRequestHandler(handler);
            IRequestDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeRequest());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeRequest>()), Times.Once);
        }

        [Fact]
        public async Task Dispatch_WhenResponseRequest_RequestHandlerIsCalledOnce()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            MockResponseRequestHandler handler = new MockResponseRequestHandler();
            services.SetupRequestHandler(handler);
            IRequestDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeResponseRequest());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeResponseRequest>()), Times.Once);
        }
    }
}
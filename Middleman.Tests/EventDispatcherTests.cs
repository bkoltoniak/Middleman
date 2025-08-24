using Middleman.Interfaces;
using Middleman.Tests.Mocks;
using Moq;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    public class EventDispatcherTests
    {
        [Fact]
        public async Task Dispatch_WhenHandlingEvent_AndNoHandlersRegistered_ShouldNotThrow()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            IEventDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeEvent());
        }

        [Fact]
        public async Task Dispatch_WhenHandlingEvent_AndTwoHandlersRegistered_ShouldHandleEventInEveryHandler()
        {
            // Arrange.
            MockServiceProvider services = new MockServiceProvider();
            MockEventHandler handler = new MockEventHandler();
            MockEventHandler handler2 = new MockEventHandler();
            services.SetupEventHandlers(handler, handler2);
            IEventDispatcher sut = new Dispatcher(services);

            // Act.
            await sut.Dispatch(new FakeEvent());

            // Assert.
            handler.Verify(x => x.Handle(It.IsAny<FakeEvent>()), Times.Once);
            handler2.Verify(x => x.Handle(It.IsAny<FakeEvent>()), Times.Once);
        }
    }
}
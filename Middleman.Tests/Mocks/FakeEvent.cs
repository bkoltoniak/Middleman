using Middleman.Interfaces;
using Moq;
using System.Threading.Tasks;

namespace Middleman.Tests.Mocks
{
    internal class FakeEvent : IEvent
    {

    }

    internal class MockEventHandler : Mock<IEventHandler<FakeEvent>>, IEventHandler<FakeEvent>
    {
        public Task Handle(FakeEvent command)
        {
            return Object.Handle(command);
        }
    }
}
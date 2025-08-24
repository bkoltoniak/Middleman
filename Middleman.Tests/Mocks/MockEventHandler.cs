using Middleman.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleman.Tests.Mocks
{
    internal class MockEventHandler : Mock<IEventHandler<FakeEvent>>, IEventHandler<FakeEvent>
    {
        private string? _id { get; }
        private List<string>? _log { get; }

        public MockEventHandler(string? id = null, List<string>? log = null)
        {
            _id = id;
            _log = log;

            Setup(x => x.Handle(It.IsAny<FakeEvent>())).Returns<FakeEvent>(@event =>
            {
                _log?.Add(_id!);
                return Task.CompletedTask;
            });
        }

        public Task Handle(FakeEvent command)
        {
            return Object.Handle(command);
        }
    }
}
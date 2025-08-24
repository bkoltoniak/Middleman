using Middleman.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    internal class MockRequestHandler : Mock<IRequestHandler<FakeRequest>>, IRequestHandler<FakeRequest>
    {
        private string? _id { get; }
        private List<string>? _log { get; }

        public MockRequestHandler(string? id = null, List<string>? log = null)
        {
            _id = id;
            _log = log;

            Setup(x => x.Handle(It.IsAny<FakeRequest>())).Returns<FakeRequest>(request =>
            {
                _log?.Add(_id!);
                return Task.CompletedTask;
            });
        }

        public Task Handle(FakeRequest command)
        {
            return Object.Handle(command);
        }
    }

    internal class MockResponseRequestHandler : Mock<IRequestHandler<FakeResponseRequest, FakeResponse>>, IRequestHandler<FakeResponseRequest, FakeResponse>
    {
        private string? _id { get; }
        private List<string>? _log { get; }

        public MockResponseRequestHandler(string? id = null, List<string>? log = null)
        {
            _id = id;
            _log = log;

            Setup(x => x.Handle(It.IsAny<FakeResponseRequest>())).Returns<FakeResponseRequest>(request =>
            {
                _log?.Add(_id!);
                return Task.FromResult(new FakeResponse());
            });
        }

        public Task<FakeResponse> Handle(FakeResponseRequest command)
        {
            return Object.Handle(command);
        }
    }
}
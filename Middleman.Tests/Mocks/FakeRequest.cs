using Middleman.Interfaces;
using Moq;
using System.Threading.Tasks;

namespace Middleman.Tests
{
    internal class FakeRequest : IRequest
    {
    }

    internal class FakeResponse
    {

    }

    internal class FakeResponseRequest : IRequest<FakeResponse>
    {

    }

    internal class MockRequestHandler : Mock<IRequestHandler<FakeRequest>>, IRequestHandler<FakeRequest>
    {
        public Task Handle(FakeRequest command)
        {
            return Object.Handle(command);
        }
    }

    internal class MockResponseRequestHandler : Mock<IRequestHandler<FakeResponseRequest, FakeResponse>>, IRequestHandler<FakeResponseRequest, FakeResponse>
    {
        public Task<FakeResponse> Handle(FakeResponseRequest command)
        {
            return Object.Handle(command);
        }
    }
}
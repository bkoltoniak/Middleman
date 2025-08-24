using Middleman.Interfaces;

namespace Middleman.Tests
{
    internal class FakeRequest : IRequest
    {
    }

    internal class FakeResponseRequest : IRequest<FakeResponse>
    {

    }

    internal class FakeResponse
    {

    }
}
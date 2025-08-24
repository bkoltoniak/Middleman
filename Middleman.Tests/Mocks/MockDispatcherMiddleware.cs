using Middleman.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleman.Tests.Mocks
{
    internal class MockDispatcherMiddleware : Mock<IDispatcherMiddleware>, IDispatcherMiddleware
    {
        private string? _id { get; }
        private List<string>? _log { get; }

        public MockDispatcherMiddleware(string? id = null, List<string>? log = null)
        {
            _id = id;
            _log = log;

            Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<DispatcherDelegate>())).Returns<object, DispatcherDelegate>(async (message, next) =>
            {
                _log?.Add(_id!);
                var result = await next(message);
                _log?.Add(_id!);
                return result;
            });
        }

        public Task<object?> Handle(object message, DispatcherDelegate next)
        {
            return Object.Handle(message, next);
        }
    }
}

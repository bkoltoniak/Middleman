using Middleman.Interfaces;
using Middleman.Models;
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

            Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<MessageDescriptor>(), It.IsAny<DispatcherDelegate>()))
                .Returns<object, MessageDescriptor, DispatcherDelegate>(async (message, descriptor, next) =>
                {
                    _log?.Add(_id!);
                    var result = await next(message);
                    _log?.Add(_id!);
                    return result;
                });
        }

        public Task<object?> Handle(object message, MessageDescriptor messageDescriptor, DispatcherDelegate next)
        {
            return Object.Handle(message, messageDescriptor, next);
        }
    }
}

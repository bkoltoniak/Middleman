using System;

namespace Middleman.Exceptions
{
    /// <summary>
    /// Exception thrown when dispatcher cannot find a matching handler in the service provider.
    /// </summary>
    public class HandlerNotFoundException : DispatcherException
    {
        public HandlerNotFoundException(Type handlerType) :
            base($"Handler of type {handlerType.Name} was not found in the service provider.")
        {
        }
    }
}

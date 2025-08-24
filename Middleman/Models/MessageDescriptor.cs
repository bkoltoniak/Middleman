using Middleman.Enums;
using System;

namespace Middleman.Models
{
    public struct MessageDescriptor
    {
        public Type Type { get; }
        public MessageKind Kind { get; }
        public bool ProducesResponse { get; }

        private MessageDescriptor(Type type, MessageKind kind, bool producesResponse)
        {
            Type = type;
            Kind = kind;
            ProducesResponse = producesResponse;
        }

        public static MessageDescriptor EventDescriptor(Type messageType) => new MessageDescriptor(messageType, MessageKind.Event, false);
        public static MessageDescriptor Request(Type messageType) => new MessageDescriptor(messageType, MessageKind.Request, false);
        public static MessageDescriptor RequestWithResponse(Type messageType) => new MessageDescriptor(messageType, MessageKind.Request, true);
    }
}

using System;
using System.Collections.Generic;

namespace Haven.Messaging
{
	public class MessageBroker : IMessageDispatcher, IMessageSource
	{
		private readonly Dictionary<Type, ICollection<IMessageHandler>> handlers;

		public MessageBroker()
		{
			this.handlers = new Dictionary<Type, ICollection<IMessageHandler>>();
		}

		public void Dispatch<TMessage>(TMessage message)
		{
			foreach (var handler in GetHandlersOf(typeof(TMessage)))
				handler.Handle(message);
		}

		public void Subscribe(IMessageHandler handler)
		{
			foreach (var messageType in handler.SupportedMessageTypes)
				GetHandlersOf(messageType).Add(handler);
		}

		public void Unsubscribe(IMessageHandler handler)
		{
			foreach (var handlerList in handlers.Values)
				handlerList.Remove(handler);
		}

		private ICollection<IMessageHandler> GetHandlersOf(Type type)
		{
			ICollection<IMessageHandler> handlerList;
			if (!handlers.TryGetValue(type, out handlerList))
			{
				handlerList = new List<IMessageHandler>();
				handlers[type] = handlerList;
			}
			return handlerList;
		}
	}
}

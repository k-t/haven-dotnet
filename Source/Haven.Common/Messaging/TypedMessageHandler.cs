using System;
using System.Collections.Generic;

namespace Haven.Messaging
{
	public abstract class TypedMessageHandler : IMessageHandler
	{
		private readonly Dictionary<Type, Action<object>> handlers;

		protected TypedMessageHandler()
		{
			handlers = new Dictionary<Type, Action<object>>();
		}

		public bool CanHandle(object message)
		{
			return message != null && handlers.ContainsKey(message.GetType());
		}

		public void Handle(object message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			var handler = GetHandler(message.GetType());
			if (handler == null)
				throw new ArgumentException($"Unsupported message type '{message.GetType()}'", nameof(message));

			handler(message);
		}

		protected void AddHandler(Type type, Action<object> handler)
		{
			handlers[type] = handler;
		}

		protected void AddHandler<TMessage>(Action<TMessage> handler)
		{
			handlers[typeof(TMessage)] = (message) => handler((TMessage)message);
		}

		private Action<object> GetHandler(Type type)
		{
			Action<object> handler;
			return (handlers.TryGetValue(type, out handler)) ? handler : null;
		}
	}
}

using System.Collections.Generic;

namespace Haven.Messaging
{
	public class MessageBroker : IMessageDispatcher, IMessageSource
	{
		private readonly ICollection<IMessageHandler> handlers;

		public MessageBroker()
		{
			this.handlers = new List<IMessageHandler>();
		}

		public void Dispatch<TMessage>(TMessage message)
		{
			foreach (var handler in handlers)
			{
				if (handler.CanHandle(message))
					handler.Handle(message);
			}
		}

		public void Subscribe(IMessageHandler handler)
		{
			handlers.Add(handler);
		}

		public void Unsubscribe(IMessageHandler handler)
		{
			handlers.Remove(handler);
		}
	}
}

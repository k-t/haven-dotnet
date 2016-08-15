using System;
using System.Collections.Generic;

namespace Haven.Messaging
{
	public interface IMessageHandler
	{
		IEnumerable<Type> SupportedMessageTypes { get; }

		void Handle(object message);
	}
}

using System;
using Haven.Messages;
using Haven.Messaging;
using Haven.Net;

namespace TestClient
{
	public class TestMessageHandler : IMessageHandler
	{
		private readonly GameClient client;

		public TestMessageHandler(GameClient client)
		{
			this.client = client;
		}

		public bool CanHandle(object message)
		{
			// handle all possible messages
			return true;
		}

		public void Handle(object message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			if (message is ExceptionMessage)
			{
				var exceptionMessage = (ExceptionMessage)message;
				Console.WriteLine("\t>> Exception: " + exceptionMessage.Exception.Message);
			}
			else if (message is ExitMessage)
			{
				Console.WriteLine("\t>> Exit");
				client.Close();
				Console.WriteLine("Disconnected...");
			}
			else
			{
				Console.WriteLine("\t>> " + message.GetType().Name);
			}
		}
	}
}

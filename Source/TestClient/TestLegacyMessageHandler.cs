using System;
using Haven.Messages;
using Haven.Messaging;
using Haven.Net;
using Haven.Protocols.Legacy.Messages;

namespace TestClient
{
	public class TestLegacyMessageHandler : MessageHandlerBase
	{
		private static readonly Type[] MessageTypes = {
			typeof(BuffClearAll),
			typeof(BuffRemove),
			typeof(BuffUpdate),
			typeof(LoadResource),
			typeof(MapInvalidate),
			typeof(MapInvalidateGrid),
			typeof(MapInvalidateRegion),
			typeof(MapUpdateGrid),
			typeof(PartyChangeLeader),
			typeof(PartyUpdate),
			typeof(PartyUpdateMember),
			typeof(PlayMusic),
			typeof(PlaySound),
			typeof(UpdateAmbientLight),
			typeof(UpdateActions),
			typeof(UpdateAstronomy),
			typeof(UpdateCharAttributes),
			typeof(UpdateGameObject),
			typeof(UpdateGameTime),
			typeof(WidgetCreate),
			typeof(WidgetDestroy),
			typeof(WidgetMessage),
			typeof(ExitMessage),
			typeof(ExceptionMessage),
			typeof(LoadTilesets)
		};

		private readonly GameClient client;

		public TestLegacyMessageHandler(GameClient client)
		{
			this.client = client;

			foreach (var messageType in MessageTypes)
			{
				AddHandler(messageType, HandleMessage);
			}
		}

		private void HandleMessage(object message)
		{
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

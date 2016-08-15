using Haven.Net;

namespace Haven.Protocols.Hafen
{
	public class HafenClient : GameClient
	{
		public HafenClient(GameClientConfig config)
			: base(config, new HafenAuthHandlerFactory(), new HafenProtocolHandlerFactory())
		{
		}
	}
}

using Haven.Net;

namespace Haven.Protocols.Hafen
{
	public class HafenProtocolHandlerFactory : IProtocolHandlerFactory
	{
		public IProtocolHandler Create()
		{
			return new HafenProtocolHandler();
		}
	}
}

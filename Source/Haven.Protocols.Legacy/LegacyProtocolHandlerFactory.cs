using Haven.Net;

namespace Haven.Protocols.Legacy
{
	public class LegacyProtocolHandlerFactory : IProtocolHandlerFactory
	{
		public IProtocolHandler Create()
		{
			return new LegacyProtocolHandler();
		}
	}
}

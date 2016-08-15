using Haven.Net;

namespace Haven.Protocols.Legacy
{
	public class LegacyAuthHandlerFactory : IAuthHandlerFactory
	{
		public IAuthHandler Create()
		{
			return new LegacyAuthHandler();
		}
	}
}

using Haven.Net;

namespace Haven.Protocols.Hafen
{
	public class HafenAuthHandlerFactory : IAuthHandlerFactory
	{
		public IAuthHandler Create()
		{
			return new HafenAuthHandler();
		}
	}
}

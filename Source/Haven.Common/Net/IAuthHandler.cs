using System;

namespace Haven.Net
{
	public interface IAuthHandler : IDisposable
	{
		void Connect(NetworkAddress address);
		byte[] GetToken();
		AuthResult TryPassword(string userName, string password);
		AuthResult TryToken(string userName, byte[] token);
	}
}

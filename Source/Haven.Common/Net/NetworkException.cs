using System;

namespace Haven.Net
{
	public class NetworkException : Exception
	{
		public NetworkException(string message, Exception innerException)
			: base(message, innerException)
		{
			Error = ConnectionError.ConnectionError;
		}

		public NetworkException(ConnectionError error)
			: base(error.GetMessage())
		{
			Error = error;
		}

		public ConnectionError Error { get; }
	}
}

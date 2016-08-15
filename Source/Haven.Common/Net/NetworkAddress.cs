namespace Haven.Net
{
	public class NetworkAddress
	{
		private readonly string host;
		private readonly int port;

		public NetworkAddress(string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		public string Host
		{
			get { return host; }
		}

		public int Port
		{
			get { return port; }
		}
	}
}

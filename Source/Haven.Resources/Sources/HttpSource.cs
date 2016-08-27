using System.IO;
using System.Net;
using Haven.Resources.Formats.Binary;

namespace Haven.Resources
{
	public class HttpSource : IResourceSource
	{
		private readonly WebClient client;
		private readonly BinaryResourceSerializer serializer;

		public HttpSource(string url)
		{
			client = new WebClient();
			client.BaseAddress = url;
			client.Headers["User-Agent"] = "Haven/1.0";
			serializer = new BinaryResourceSerializer();
		}

		public string Url
		{
			get { return client.BaseAddress; }
		}

		public string Description
		{
			get { return $"[Http] {client.BaseAddress}"; }
		}

		public Resource Get(string resourceName)
		{
			var data = client.DownloadData(resourceName + ".res");
			return serializer.Deserialize(new MemoryStream(data));
		}

		public void Dispose()
		{
			client.Dispose();
		}
	}
}

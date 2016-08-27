namespace Haven.Resources
{
	public class AudioLayer
	{
		public string Id { get; set; } = "cl";

		public byte[] Data { get; set; }

		public double BaseVolume { get; set; } = 1.0;
	}
}

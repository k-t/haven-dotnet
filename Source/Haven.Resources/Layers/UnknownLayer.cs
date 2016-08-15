namespace Haven.Resources
{
	public class UnknownLayer
	{
		public UnknownLayer(string layerName, byte[] bytes)
		{
			LayerName = layerName;
			Bytes = bytes;
		}

		public string LayerName { get; }
		public byte[] Bytes { get; }
	}
}


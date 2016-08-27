namespace Haven.Resources
{
	public class UnknownLayer
	{
		public UnknownLayer(string layerName, byte[] data)
		{
			LayerName = layerName;
			Data = data;
		}

		public string LayerName { get; }

		public byte[] Data { get; }
	}
}


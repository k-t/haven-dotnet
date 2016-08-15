namespace Haven.Resources
{
	public class ImageLayer
	{
		public short Id { get; set; }
		public short Z { get; set; }
		public short SubZ { get; set; }
		public byte[] Data { get; set; }
		public Point2D Offset { get; set; }
		public bool IsLayered { get; set; }
	}
}


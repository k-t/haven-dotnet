namespace Haven.Resources
{
	public class TileLayer
	{
		public byte Id { get; set; }
		public ushort Weight { get; set; }
		public char Type { get; set; }
		public byte[] ImageData { get; set; }
	}
}

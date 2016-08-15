namespace Haven.Protocols.Legacy.Messages
{
	public class MapUpdateGrid
	{
		public Point2D Coord { get; set; }

		public string MinimapName { get; set; }

		public byte[] Tiles { get; set; }

		public int[] Overlays { get; set; }
	}
}
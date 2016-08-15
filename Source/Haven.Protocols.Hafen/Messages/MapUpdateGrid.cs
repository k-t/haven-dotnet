namespace Haven.Protocols.Hafen.Messages
{
	public class MapUpdateGrid
	{
		public long Id { get; set; }

		public Point2D Coord { get; set; }

		public string MinimapName { get; set; }

		public byte[] Tiles { get; set; }

		public short[] Z { get; set; }

		public int[] Overlays { get; set; }

		public TilesetBinding[] Tilesets { get; set; }
	}

	public class TilesetBinding
	{
		public byte Id { get; set; }

		public string Name { get; set; }

		public ushort Version { get; set; }
	}
}
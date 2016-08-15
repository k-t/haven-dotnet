using System.Collections.Generic;

namespace Haven.Protocols.Legacy.Messages
{
	public class LoadTilesets
	{
		public IEnumerable<TilesetBinding> Tilesets { get; set; }
	}

	public class TilesetBinding
	{
		public byte Id { get; set; }

		public string Name { get; set; }

		public ushort Version { get; set; }
	}
}
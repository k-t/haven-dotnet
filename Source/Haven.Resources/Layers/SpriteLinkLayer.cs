using System;
using System.Collections.Generic;

namespace Haven.Resources
{
	public class SpriteLinkLayer
	{
		public byte Version { get; set; }

		public SortedDictionary<short, SpriteLink> Links { get; set; }

		public short DefaultLinkId { get; set; }
	}

	public abstract class SpriteLink
	{
		public class Resource : SpriteLink
		{
			public string Name { get; set; }

			public ushort Version { get; set; }
		}

		public class Tile : SpriteLink
		{
			public short DefaultLinkId { get; set; }

			public TileSubLink[] SubLinks { get; set; }
		}
	}

	public class TileSubLink
	{
		public string Tag { get; set; }

		/// <summary>
		/// Reference to another link in the layer.
		/// </summary>
		public short SubLinkId { get; set; }
	}

}

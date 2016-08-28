using System;
using System.Collections.Generic;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public class SpriteLinkLayerHandler : GenericLayerHandler<SpriteLinkLayer>
	{
		private const byte Version = 1;

		public SpriteLinkLayerHandler() : base("slink")
		{
		}

		protected override SpriteLinkLayer Deserialize(BinaryDataReader reader)
		{
			var version = reader.ReadByte();
			if (version != Version)
				throw new ResourceException($"Unknown spritelink version: '{version}'");

			var links = new SortedDictionary<short, SpriteLink>();
			while (true)
			{
				var id = reader.ReadInt16();
				if (id < 0)
					break;

				var type = reader.ReadChar();
				switch (type)
				{
					case 't':
						links[id] = ReadTileLink(reader);
						break;
					case 'r':
						links[id] = ReadResourceLink(reader);
						break;
					default:
						throw new ResourceException($"Unknown spritelink type: '{type}'");
				}
			}

			return new SpriteLinkLayer {
				Version = version,
				Links = links,
				DefaultLinkId = reader.ReadInt16()
			};
		}

		protected override void Serialize(BinaryDataWriter writer, SpriteLinkLayer layer)
		{
			writer.Write(Version);
			foreach (var entry in layer.Links)
			{
				writer.Write(entry.Key);

				var resourceLink = entry.Value as SpriteLink.Resource;
				if (resourceLink != null)
				{
					writer.Write('r');
					WriteResourceLink(writer, resourceLink);
					continue;
				}

				var tileLink = entry.Value as SpriteLink.Tile;
				if (tileLink != null)
				{
					writer.Write('t');
					WriteTileLink(writer, tileLink);
					continue;
				}

				throw new Exception($"Unsupported spritelink type: '{entry.Value.GetType().Name}'");
			}
			writer.Write((short)-1);
			writer.Write(layer.DefaultLinkId);
		}

		private static SpriteLink.Resource ReadResourceLink(BinaryDataReader reader)
		{
			var name = reader.ReadCString();
			var version = reader.ReadUInt16();
			return new SpriteLink.Resource { Name = name, Version = version };
		}

		private static void WriteResourceLink(BinaryDataWriter writer, SpriteLink.Resource link)
		{
			writer.WriteCString(link.Name);
			writer.Write(link.Version);
		}

		private static SpriteLink.Tile ReadTileLink(BinaryDataReader reader)
		{
			var size = reader.ReadByte();

			var links = new TileSubLink[size];
			for (int i = 0; i < size; i++)
			{
				var tag = reader.ReadCString();
				var id = reader.ReadInt16();
				links[i] = new TileSubLink { Tag = tag, SubLinkId = id };
			}

			var defaultLinkId = reader.ReadInt16();

			return new SpriteLink.Tile { DefaultLinkId = defaultLinkId, SubLinks = links };
		}

		private static void WriteTileLink(BinaryDataWriter writer, SpriteLink.Tile link)
		{
			writer.Write((byte)link.SubLinks.Length);
			foreach (var subLink in link.SubLinks)
			{
				writer.WriteCString(subLink.Tag);
				writer.Write(subLink.SubLinkId);
			}
			writer.Write(link.DefaultLinkId);
		}
	}
}

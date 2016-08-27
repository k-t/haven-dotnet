using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class Tileset2LayerHandler : GenericLayerHandler<Tileset2Layer>
	{
		private const byte Tiler = 0;
		private const byte Flavor = 1;
		private const byte Tags = 2;

		public Tileset2LayerHandler() : base("tileset2")
		{
		}

		protected override Tileset2Layer Deserialize(BinaryDataReader reader)
		{
			var data = new Tileset2Layer();
			while (reader.HasRemaining)
			{
				var part = reader.ReadByte();
				switch (part)
				{
					case Tiler:
						data.TilerName = reader.ReadCString();
						data.TilerAttributes = reader.ReadList();
						break;
					case Flavor:
						var flavorCount = reader.ReadUInt16();
						data.FlavorDensity = reader.ReadUInt16();
						data.FlavorObjects = new FlavorObjectData[flavorCount];
						for (int i = 0; i < flavorCount; i++)
						{
							var fob = new FlavorObjectData();
							fob.Resource = new ResourceRef(reader.ReadCString(), reader.ReadUInt16());
							fob.Weight = reader.ReadByte();
							data.FlavorObjects[i] = fob;
						}
						break;
					case Tags:
						data.Tags = new string[reader.ReadByte()];
						for (int i = 0; i < data.Tags.Length; i++)
							data.Tags[i] = reader.ReadCString();
						break;
					default:
						throw new ResourceException($"Invalid tileset part {part}");
				}
			}
			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, Tileset2Layer data)
		{
			// tiler
			writer.Write(Tiler);
			writer.WriteCString(data.TilerName);
			writer.WriteList(data.TilerAttributes);
			// flavor
			writer.Write(Flavor);
			writer.Write((ushort)data.FlavorObjects.Length);
			writer.Write(data.FlavorDensity);
			foreach (var fob in data.FlavorObjects)
			{
				writer.WriteCString(fob.Resource.Name);
				writer.Write(fob.Resource.Version);
				writer.Write(fob.Weight);
			}
			// tags
			writer.Write(Tags);
			writer.Write((byte)data.Tags.Length);
			foreach (var tag in data.Tags)
				writer.WriteCString(tag);
		}
	}
}

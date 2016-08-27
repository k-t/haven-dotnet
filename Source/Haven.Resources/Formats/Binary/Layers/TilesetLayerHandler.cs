using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class TilesetLayerHandler : GenericLayerHandler<TilesetLayer>
	{
		public TilesetLayerHandler() : base("tileset")
		{
		}

		protected override TilesetLayer Deserialize(BinaryDataReader reader)
		{
			var tileset = new TilesetLayer();
			tileset.HasTransitions = reader.ReadBoolean();
			var flavorCount = reader.ReadUInt16();
			tileset.FlavorDensity = reader.ReadUInt16();
			tileset.FlavorObjects = new FlavorObjectData[flavorCount];
			for (int i = 0; i < flavorCount; i++)
			{
				var fob = new FlavorObjectData();
				fob.Resource = new ResourceRef(reader.ReadCString(), reader.ReadUInt16());
				fob.Weight = reader.ReadByte();
				tileset.FlavorObjects[i] = fob;
			}
			return tileset;
		}

		protected override void Serialize(BinaryDataWriter writer, TilesetLayer tileset)
		{
			writer.Write(tileset.HasTransitions);
			writer.Write((ushort)tileset.FlavorObjects.Length);
			writer.Write(tileset.FlavorDensity);
			foreach (var fob in tileset.FlavorObjects)
			{
				writer.WriteCString(fob.Resource.Name);
				writer.Write(fob.Resource.Version);
				writer.Write(fob.Weight);
			}
		}
	}
}
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class TileLayerHandler : GenericLayerHandler<TileLayer>
	{
		public TileLayerHandler() : base("tile")
		{
		}

		protected override TileLayer Deserialize(BinaryDataReader reader)
		{
			var tile = new TileLayer();
			tile.Type = reader.ReadChar();
			tile.Id = reader.ReadByte();
			tile.Weight = reader.ReadUInt16();
			tile.ImageData = reader.ReadRemaining();
			return tile;
		}

		protected override void Serialize(BinaryDataWriter writer, TileLayer tile)
		{
			writer.Write(tile.Type);
			writer.Write(tile.Id);
			writer.Write(tile.Weight);
			writer.Write(tile.ImageData);
		}
	}
}
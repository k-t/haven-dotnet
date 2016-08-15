using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public class UnknownLayerHandler : GenericLayerHandler<UnknownLayer>
	{
		public UnknownLayerHandler(UnknownLayer layer) : this(layer.LayerName)
		{
		}

		public UnknownLayerHandler(string layerName) : base(layerName)
		{
		}

		protected override UnknownLayer Deserialize(BinaryDataReader reader)
		{
			return new UnknownLayer(LayerName, reader.ReadRemaining());
		}

		protected override void Serialize(BinaryDataWriter writer, UnknownLayer layer)
		{
			writer.Write(layer.Bytes);
		}
	}
}

using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class ImageLayerHandler : GenericLayerHandler<ImageLayer>
	{
		public ImageLayerHandler() : base("image")
		{
		}

		protected override ImageLayer Deserialize(BinaryDataReader reader)
		{
			var img = new ImageLayer();
			img.Z = reader.ReadInt16();
			img.SubZ = reader.ReadInt16();
			/* Obsolete flag 1: Layered */
			img.IsLayered = reader.ReadBoolean();
			img.Id = reader.ReadInt16();
			img.Offset = reader.ReadInt16Coord();
			img.Data = reader.ReadRemaining();
			return img;
		}

		protected override void Serialize(BinaryDataWriter writer, ImageLayer img)
		{
			writer.Write(img.Z);
			writer.Write(img.SubZ);
			writer.Write(img.IsLayered);
			writer.Write(img.Id);
			writer.WriteInt16Coord(img.Offset);
			writer.Write(img.Data);
		}
	}
}
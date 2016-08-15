using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class NinepatchLayerHandler : GenericLayerHandler<NinepatchLayer>
	{
		public NinepatchLayerHandler() : base("ninepatch")
		{
		}

		protected override NinepatchLayer Deserialize(BinaryDataReader reader)
		{
			return new NinepatchLayer {
				Top = reader.ReadByte(),
				Bottom = reader.ReadByte(),
				Left = reader.ReadByte(),
				Right = reader.ReadByte()
			};
		}

		protected override void Serialize(BinaryDataWriter writer, NinepatchLayer ninepatch)
		{
			writer.Write(ninepatch.Top);
			writer.Write(ninepatch.Bottom);
			writer.Write(ninepatch.Left);
			writer.Write(ninepatch.Right);
		}
	}
}
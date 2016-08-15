using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class FoodEventLayerHandler : GenericLayerHandler<FoodEventLayer>
	{
		private const byte Version = 1;

		public FoodEventLayerHandler() : base("foodev")
		{
		}

		protected override FoodEventLayer Deserialize(BinaryDataReader reader)
		{
			var version = reader.ReadByte();
			if (version != 1)
				throw new ResourceException($"Unknown foodev version: {version}");

			var data = new FoodEventLayer();
			data.Color = reader.ReadColor();
			data.Name = reader.ReadCString();
			data.Sort = reader.ReadInt16();
			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, FoodEventLayer data)
		{
			writer.Write(Version);
			writer.WriteColor(data.Color);
			writer.WriteCString(data.Name);
			writer.Write(data.Sort);
		}
	}
}

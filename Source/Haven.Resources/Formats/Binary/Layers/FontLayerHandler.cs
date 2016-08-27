using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class FontLayerHandler : GenericLayerHandler<FontLayer>
	{
		private const byte Version = 1;

		public FontLayerHandler() : base("font")
		{
		}

		protected override FontLayer Deserialize(BinaryDataReader reader)
		{
			var version = reader.ReadByte();
			if (version != 1)
				throw new ResourceException($"Unknown font layer version: {version}");

			var data = new FontLayer();
			data.Type = reader.ReadByte();
			switch (data.Type)
			{
				case 0:
					data.Data = reader.ReadRemaining();
					break;
				default:
					throw new ResourceException($"Unknown font type: {data.Type}");
			}
			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, FontLayer data)
		{
			writer.Write(Version);
			writer.Write(data.Type);
			writer.Write(data.Data);
		}
	}
}
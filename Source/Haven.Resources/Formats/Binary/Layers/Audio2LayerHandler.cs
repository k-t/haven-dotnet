using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class Audio2LayerHandler : GenericLayerHandler<AudioLayer>
	{
		private const byte Version = 2;

		public Audio2LayerHandler() : base("audio2")
		{
		}

		protected override AudioLayer Deserialize(BinaryDataReader reader)
		{
			var version = reader.ReadByte();
			if (version != 1 && version != 2)
				throw new ResourceException($"Unknown audio layer version: {version}");

			var data = new AudioLayer();
			data.Id = reader.ReadCString();
			data.BaseVolume = (version == 2) ? reader.ReadUInt16() / 1000.0 : 1.0;
			data.Bytes = reader.ReadRemaining();
			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, AudioLayer audio)
		{
			writer.Write(Version);
			writer.WriteCString(audio.Id);
			writer.Write((ushort)(audio.BaseVolume * 1000));
			writer.Write(audio.Bytes);
		}
	}
}

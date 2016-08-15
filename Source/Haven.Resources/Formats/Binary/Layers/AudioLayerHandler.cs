using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class AudioLayerHandler : GenericLayerHandler<AudioLayer>
	{
		public AudioLayerHandler() : base("audio")
		{
		}

		protected override AudioLayer Deserialize(BinaryDataReader reader)
		{
			return new AudioLayer { Id = "cl", Bytes = reader.ReadRemaining() };
		}

		protected override void Serialize(BinaryDataWriter writer, AudioLayer audio)
		{
			writer.Write(audio.Bytes);
		}
	}
}

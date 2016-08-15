using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class MidiLayerHandler : GenericLayerHandler<MidiLayer>
	{
		public MidiLayerHandler() : base("midi")
		{
		}

		protected override MidiLayer Deserialize(BinaryDataReader reader)
		{
			var bytes = reader.ReadRemaining();
			return new MidiLayer { Bytes = bytes };
		}

		protected override void Serialize(BinaryDataWriter writer, MidiLayer midi)
		{
			writer.Write(midi.Bytes);
		}
	}
}

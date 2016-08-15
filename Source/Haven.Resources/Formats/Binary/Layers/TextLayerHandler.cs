using System.Text;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class TextLayerHandler : GenericLayerHandler<TextLayer>
	{
		public TextLayerHandler() : base("pagina")
		{
		}

		protected override TextLayer Deserialize(BinaryDataReader reader)
		{
			var text = Encoding.UTF8.GetString(reader.ReadRemaining());
			return new TextLayer { Text =  text };
		}

		protected override void Serialize(BinaryDataWriter writer, TextLayer text)
		{
			writer.Write(Encoding.UTF8.GetBytes(text.Text));
		}
	}
}
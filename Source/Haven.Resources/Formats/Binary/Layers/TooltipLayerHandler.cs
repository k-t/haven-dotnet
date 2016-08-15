using System.Text;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class TooltipLayerHandler : GenericLayerHandler<TooltipLayer>
	{
		public TooltipLayerHandler() : base("tooltip")
		{
		}

		protected override TooltipLayer Deserialize(BinaryDataReader reader)
		{
			var text = Encoding.UTF8.GetString(reader.ReadRemaining());
			return new TooltipLayer { Text = text };
		}

		protected override void Serialize(BinaryDataWriter writer, TooltipLayer tooltip)
		{
			writer.Write(Encoding.UTF8.GetBytes(tooltip.Text));
		}
	}
}
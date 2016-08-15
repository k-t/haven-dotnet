using System.Text;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TooltipLayerHandler : GenericLayerHandler<TooltipLayer>
	{
		private const string TextFileKey = "text";

		public TooltipLayerHandler() : base("tooltip")
		{
		}

		protected override void Init(IniLayer layer, TooltipLayer data)
		{
			layer.Files[TextFileKey] = ".txt";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = keys.GetString("file");

			var tooltip = new TooltipLayer();
			tooltip.Text = Encoding.UTF8.GetString(fileSource.Read(fileName));

			layer.Files[TextFileKey] = fileName;
			layer.Data = tooltip;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = layer.Files[TextFileKey];
			var tooltip = (TooltipLayer)layer.Data;
			fileSource.Write(fileName, Encoding.UTF8.GetBytes(tooltip.Text));
			keys.Add("file", fileName);
		}
	}
}

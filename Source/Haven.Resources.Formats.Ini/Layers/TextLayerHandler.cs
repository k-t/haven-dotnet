using System.Text;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TextLayerHandler : GenericLayerHandler<TextLayer>
	{
		private const string TextFileKey = "text";

		public TextLayerHandler() : base("pagina")
		{
		}

		protected override void Init(IniLayer layer, TextLayer data)
		{
			layer.Files[TextFileKey] = ".txt";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = keys.GetString("file");

			var text = new TextLayer();
			text.Text = Encoding.UTF8.GetString(fileSource.Read(fileName));

			layer.Files[TextFileKey] = fileName;
			layer.Data = text;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = layer.Files[TextFileKey];
			var text = (TextLayer)layer.Data;
			fileSource.Write(fileName, Encoding.UTF8.GetBytes(text.Text));
			keys.Add("file", fileName);
		}
	}
}

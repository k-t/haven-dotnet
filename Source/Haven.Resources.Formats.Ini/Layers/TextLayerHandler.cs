using System.Collections.Generic;
using System.Text;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TextLayerHandler : GenericLayerHandler<TextLayer>
	{
		private const string TextFileKey = "file";
		private static readonly string[] FileKeys = { TextFileKey };

		public TextLayerHandler() : base("pagina")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, TextLayer data)
		{
			switch (externalFileKey)
			{
				case TextFileKey:
					return ".txt";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override TextLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new TextLayer
			{
				Text = Encoding.UTF8.GetString(context.LoadExternalFile(TextFileKey))
			};
		}

		protected override void Save(IniKeyCollection iniData, TextLayer data, LayerHandlerContext context)
		{
			context.SaveExternalFile(TextFileKey, Encoding.UTF8.GetBytes(data.Text));
		}
	}
}

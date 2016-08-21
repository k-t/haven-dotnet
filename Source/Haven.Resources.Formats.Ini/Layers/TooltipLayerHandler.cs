using System.Collections.Generic;
using System.Text;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TooltipLayerHandler : GenericLayerHandler<TooltipLayer>
	{
		private const string TextFileKey = "text";
		private static readonly string[] FileKeys = { TextFileKey };

		public TooltipLayerHandler() : base("tooltip")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, TooltipLayer data)
		{
			switch (externalFileKey)
			{
				case TextFileKey:
					return ".txt";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override TooltipLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new TooltipLayer
			{
				Text = Encoding.UTF8.GetString(context.LoadExternalFile(TextFileKey))
			};
		}

		protected override void Save(IniKeyCollection iniData, TooltipLayer data, LayerHandlerContext context)
		{
			context.SaveExternalFile(TextFileKey, Encoding.UTF8.GetBytes(data.Text));
		}
	}
}

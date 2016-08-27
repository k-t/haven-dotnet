using System.Collections.Generic;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class FontLayerHandler : GenericLayerHandler<FontLayer>
	{
		private const string FontFileKey = "file";
		private static readonly string[] FileKeys = { FontFileKey };

		public FontLayerHandler() : base("font")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, FontLayer data)
		{
			switch (externalFileKey)
			{
				case FontFileKey:
					return ".ttf";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override FontLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new FontLayer { Data = context.LoadExternalFile(FontFileKey) };
		}

		protected override void Save(IniKeyCollection iniData, FontLayer data, LayerHandlerContext context)
		{
			context.SaveExternalFile(FontFileKey, data.Data);
		}
	}
}

using System.Collections.Generic;
using Haven.Resources.Formats.Ini.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class TileLayerHandler : GenericLayerHandler<TileLayer>
	{
		private const string ImageFileKey = "image";
		private static readonly string[] FileKeys = { ImageFileKey };

		public TileLayerHandler() : base("tile")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, TileLayer data)
		{
			switch (externalFileKey)
			{
				case ImageFileKey:
					return ImageUtils.GetImageFileExtension(data.ImageData) ?? ".image";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override TileLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new TileLayer
			{
				ImageData = context.LoadExternalFile(ImageFileKey),
				Type = iniData.GetChar("type"),
				Id = iniData.GetByte("id"),
				Weight = iniData.GetUInt16("weight", 0)
			};
		}

		protected override void Save(IniKeyCollection iniData, TileLayer data, LayerHandlerContext context)
		{
			iniData.Add("type", data.Type);
			iniData.Add("id", data.Id);
			iniData.Add("weight", data.Weight);

			context.SaveExternalFile(ImageFileKey, data.ImageData);
		}
	}
}

using System.Collections.Generic;
using Haven.Resources.Formats.Ini.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class ImageLayerHandler : GenericLayerHandler<ImageLayer>
	{
		private const string ImageFileKey = "file";
		private static readonly string[] FileKeys = { ImageFileKey };

		public ImageLayerHandler() : base("image")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, ImageLayer data)
		{
			switch (externalFileKey)
			{
				case ImageFileKey:
					return ImageUtils.GetImageFileExtension(data.Data) ?? ".image";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override ImageLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new ImageLayer
			{
				Id = iniData.GetInt16("id", -1),
				Z = iniData.GetInt16("z", 0),
				SubZ = iniData.GetInt16("subz", 0),
				Offset = iniData.GetPoint("off", Point2D.Empty),
				Data = context.LoadExternalFile(ImageFileKey),
			};
		}

		protected override void Save(IniKeyCollection iniData, ImageLayer data, LayerHandlerContext context)
		{
			iniData.Add("id", data.Id);
			iniData.Add("z", data.Z);
			iniData.Add("subz", data.SubZ);
			iniData.Add("off", data.Offset);

			context.SaveExternalFile(ImageFileKey, data.Data);
		}
	}
}

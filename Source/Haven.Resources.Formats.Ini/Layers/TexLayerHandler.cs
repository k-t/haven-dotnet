using System.Collections.Generic;
using Haven.Resources.Formats.Ini.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TexLayerHandler : GenericLayerHandler<TexLayer>
	{
		private const string DefaultImageExtension = ".img";
		private const string ImageFileKey = "image";
		private const string MaskFileKey = "mask";
		private static readonly string[] FileKeys = { ImageFileKey, MaskFileKey };

		public TexLayerHandler() : base("tex")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, TexLayer data)
		{
			switch (externalFileKey)
			{
				case ImageFileKey:
					return ImageUtils.GetImageFileExtension(data.ImageData) ?? DefaultImageExtension;
				case MaskFileKey:
					return (data.MaskImageData != null)
						? ImageUtils.GetImageFileExtension(data.MaskImageData) ?? DefaultImageExtension
						: null;
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override TexLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			var data = new TexLayer();
			data.Id = iniData.GetInt16("id", -1);
			data.ImageData = context.LoadExternalFile(ImageFileKey);
			data.MaskImageData = context.HasExternalFile(MaskFileKey) ? context.LoadExternalFile(MaskFileKey) : null;
			data.Offset = iniData.GetPoint("off", Point2D.Empty);
			data.Size = iniData.GetPoint("size");
			data.Mipmap = iniData.GetEnum("mipmap", TexMipmap.None);
			data.MagFilter = iniData.GetEnum("magfilter", TexMagFilter.Nearest);

			var defaultMinFilter = (data.Mipmap != TexMipmap.None)
				? TexMinFilter.LinearMipmapLinear
				: TexMinFilter.Linear;
			data.MinFilter = iniData.GetEnum("minfilter", defaultMinFilter);

			return data;
		}

		protected override void Save(IniKeyCollection iniData, TexLayer data, LayerHandlerContext context)
		{
			context.SaveExternalFile(ImageFileKey, data.ImageData);

			if (data.MaskImageData != null)
				context.SaveExternalFile(MaskFileKey, data.MaskImageData);

			iniData.Add("id", data.Id);
			iniData.Add("off", data.Offset);
			iniData.Add("size", data.Size);
			iniData.Add("magfilter", data.MagFilter.ToString());
			iniData.Add("minfilter", data.MinFilter.ToString());
			iniData.Add("mipmap", data.Mipmap.ToString());
		}
	}
}

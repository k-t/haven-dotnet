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
					return ImageUtils.GetImageFileExtension(data.Image) ?? DefaultImageExtension;
				case MaskFileKey:
					return (data.Mask != null)
						? ImageUtils.GetImageFileExtension(data.Mask) ?? DefaultImageExtension
						: null;
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override TexLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			var data = new TexLayer();
			data.Id = iniData.GetInt16("id", -1);
			data.Image = context.LoadExternalFile(ImageFileKey);
			data.Mask = context.HasExternalFile(ImageFileKey) ? context.LoadExternalFile(ImageFileKey) : null;
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
			context.SaveExternalFile(ImageFileKey, data.Image);

			if (data.Mask != null)
				context.SaveExternalFile(MaskFileKey, data.Mask);

			iniData.Add("id", data.Id);
			iniData.Add("off", data.Offset);
			iniData.Add("size", data.Size);
			iniData.Add("magfilter", data.MagFilter.ToString());
			iniData.Add("minfilter", data.MinFilter.ToString());
			iniData.Add("mipmap", data.Mipmap.ToString());
		}
	}
}

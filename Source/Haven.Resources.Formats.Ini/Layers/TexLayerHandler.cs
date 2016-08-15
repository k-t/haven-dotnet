using Haven.Resources.Formats.Ini.Utils;
using Haven.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TexLayerHandler : GenericLayerHandler<TexLayer>
	{
		private const string TexSectionName = "tex";
		private const string ImageFileKey = "image";
		private const string MaskFileKey = "mask";

		public TexLayerHandler() : base(TexSectionName)
		{
		}

		protected override void Init(IniLayer layer, TexLayer data)
		{
			layer.Files[ImageFileKey] = ImageUtils.GetImageFileExtension(data.Image) ?? ".image";
			if (data.Mask != null)
				layer.Files[MaskFileKey] = ImageUtils.GetImageFileExtension(data.Mask) ?? ".image";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var imageFileName = keys.GetString("image");
			var maskFileName = keys.GetString("mask", null);

			var data = new TexLayer();
			data.Id = keys.GetInt16("id", -1);
			data.Image = fileSource.Read(imageFileName);
			data.Mask = !string.IsNullOrEmpty(maskFileName) ? fileSource.Read(maskFileName) : null;
			data.Offset = keys.GetPoint("off", Point2D.Empty);
			data.Size = keys.GetPoint("size");
			data.Mipmap = keys.GetEnum("mipmap", TexMipmap.None);
			data.MagFilter = keys.GetEnum("magfilter", TexMagFilter.Nearest);

			var defaultMinFilter = (data.Mipmap != TexMipmap.None)
				? TexMinFilter.LinearMipmapLinear
				: TexMinFilter.Linear;
			data.MinFilter = keys.GetEnum("minfilter", defaultMinFilter);

			layer.Files[ImageFileKey] = imageFileName;
			layer.Files[MaskFileKey] = maskFileName;
			layer.Data = data;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (TexLayer)layer.Data;

			var imageFileName = layer.Files[ImageFileKey];
			keys.Add("image", imageFileName);
			fileSource.Write(imageFileName, data.Image);

			var maskFileName = layer.Files[MaskFileKey];
			if (!string.IsNullOrEmpty(maskFileName))
			{
				keys.Add("mask", maskFileName);
				fileSource.Write(maskFileName, data.Mask);
			}

			keys.Add("id", data.Id);
			keys.Add("off", data.Offset);
			keys.Add("size", data.Size);
			keys.Add("magfilter", data.MagFilter.ToString());
			keys.Add("minfilter", data.MinFilter.ToString());
			keys.Add("mipmap", data.Mipmap.ToString());
		}
	}
}

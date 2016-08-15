using Haven.Resources.Formats.Ini.Utils;
using Haven.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class ImageLayerHandler : GenericLayerHandler<ImageLayer>
	{
		private const string ImageSectionName = "image";
		private const string ImageFileKey = "image";

		public ImageLayerHandler() : base(ImageSectionName)
		{
		}

		protected override void Init(IniLayer layer, ImageLayer data)
		{
			layer.Files[ImageFileKey] = ImageUtils.GetImageFileExtension(data.Data) ?? ".image";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = keys.GetString("file");

			var data = new ImageLayer();
			data.Data = fileSource.Read(fileName);
			data.Id = keys.GetInt16("id", -1);
			data.Z = keys.GetInt16("z", 0);
			data.SubZ = keys.GetInt16("subz", 0);
			data.Offset = keys.GetPoint("off", Point2D.Empty);

			layer.Data = data;
			layer.Files[ImageFileKey] = fileName;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (ImageLayer)layer.Data;

			var fileName = layer.Files[ImageFileKey];
			keys.Add("file", fileName);
			keys.Add("id", data.Id);
			keys.Add("z", data.Z);
			keys.Add("subz", data.SubZ);
			keys.Add("off", data.Offset);
			fileSource.Write(fileName, data.Data);
		}
	}
}

using Haven.Resources.Formats.Ini.Utils;
using Haven.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class TileLayerHandler : GenericLayerHandler<TileLayer>
	{
		private const string TileSectionName = "tile";
		private const string ImageFileKey = "image";

		public TileLayerHandler() : base(TileSectionName)
		{
		}

		protected override void Init(IniLayer layer, TileLayer data)
		{
			layer.Files[ImageFileKey] = ImageUtils.GetImageFileExtension(data.ImageData) ?? ".image";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var imageFileName = keys.GetString("image");

			var data = new TileLayer();
			data.ImageData = fileSource.Read(imageFileName);
			data.Type = keys.GetChar("type");
			data.Id = keys.GetByte("id");
			data.Weight = keys.GetUInt16("weight", 0);

			layer.Data = data;
			layer.Files[ImageFileKey] = imageFileName;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (TileLayer)layer.Data;

			var imageFileName = layer.Files[ImageFileKey];
			keys.Add("image", imageFileName);
			keys.Add("type", data.Type);
			keys.Add("id", data.Id);
			keys.Add("weight", data.Weight);
			fileSource.Write(imageFileName, data.ImageData);
		}
	}
}

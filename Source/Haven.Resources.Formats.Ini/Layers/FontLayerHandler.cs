using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class FontLayerHandler : GenericLayerHandler<FontLayer>
	{
		private const string FontSectionName = "font";
		private const string DataFileKey = "data";

		public FontLayerHandler() : base(FontSectionName)
		{
		}

		protected override void Init(IniLayer layer, FontLayer data)
		{
			layer.Files[DataFileKey] = ".ttf";
		}

		protected override void Load(IniLayer layer, IniKeyCollection attrs, IFileSource fileSource)
		{
			var fileName = attrs["file"].Value;

			var data = new FontLayer();
			data.Bytes = fileSource.Read(fileName);

			layer.Data = data;
			layer.Files[DataFileKey] = fileName;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (FontLayer)layer.Data;

			var fileName = layer.Files[DataFileKey];
			keys.Add("file", fileName);
			fileSource.Write(fileName, data.Bytes);
		}
	}
}

using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class AudioLayerHandler : GenericLayerHandler<AudioLayer>
	{
		private const string AudioFileKey = "audio";

		public AudioLayerHandler() : base("audio")
		{
		}

		protected override void Init(IniLayer layer, AudioLayer data)
		{
			layer.Files[AudioFileKey] = ".ogg";
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = keys.GetString("file");

			var data = new AudioLayer();
			data.Id = keys.GetString("id");
			data.BaseVolume = keys.GetDouble("volume", 1.0);
			data.Bytes = fileSource.Read(fileName);

			layer.Data = data;
			layer.Files[AudioFileKey] = fileName;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var fileName = layer.Files[AudioFileKey];

			var data = (AudioLayer)layer.Data;
			keys.Add("id", data.Id);
			keys.Add("volume", data.BaseVolume);
			keys.Add("file", fileName);

			fileSource.Write(fileName, data.Bytes);
		}
	}
}

using System.Collections.Generic;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class AudioLayerHandler : GenericLayerHandler<AudioLayer>
	{
		private const string AudioFileKey = "audio";
		private static readonly string[] FileKeys = { AudioFileKey };

		public AudioLayerHandler() : base("audio")
		{
		}

		public override IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		protected override string GetExternalFileExtension(string externalFileKey, AudioLayer data)
		{
			switch (externalFileKey)
			{
				case AudioFileKey:
					return ".ogg";
			}
			return base.GetExternalFileExtension(externalFileKey, data);
		}

		protected override AudioLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new AudioLayer
			{
				Id = iniData.GetString("id"),
				BaseVolume = iniData.GetDouble("volume", 1.0),
				Data = context.LoadExternalFile(AudioFileKey)
			};
		}

		protected override void Save(IniKeyCollection iniData, AudioLayer data, LayerHandlerContext context)
		{
			iniData.Add("id", data.Id);
			iniData.Add("volume", data.BaseVolume);

			context.SaveExternalFile(AudioFileKey, data.Data);
		}
	}
}

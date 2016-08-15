using System;
using System.IO;
using Haven.Resources.Formats.Binary;
using Haven.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class BinLayerHandler : IIniLayerHandler
	{
		public const string Prefix = "bin/";
		private const string DataFileKey = "data";

		private readonly IBinaryLayerHandler binaryHandler;
		private readonly string sectionName;

		public string SectionName
		{
			get { return sectionName; }
		}

		public Type Type
		{
			get { return binaryHandler.LayerType; }
		}

		public BinLayerHandler(IBinaryLayerHandler binaryHandler)
		{
			this.binaryHandler = binaryHandler;
			this.sectionName = $"{Prefix}{binaryHandler.LayerName}";
		}

		public IniLayer Create(object data)
		{
			var layer = new IniLayer();
			layer.Data = data;
			layer.Files[DataFileKey] = "data." + binaryHandler.LayerName;
			return layer;
		}

		public IniLayer Load(IniKeyCollection keys, IFileSource fileSource)
		{
			var layer = new IniLayer();
			var fileName = keys.GetString("file");
			layer.Files[DataFileKey] = fileName;
			using (var ms = new MemoryStream(fileSource.Read(fileName)))
			using (var buffer = new BinaryDataReader(ms))
				layer.Data = binaryHandler.Deserialize(buffer);
			return layer;
		}

		public void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			if (layer.Data == null)
				throw new InvalidOperationException();

			var fileName = layer.Files[DataFileKey];
			keys.Add("file", fileName);
			using (var ms = new MemoryStream())
			using (var buffer = new BinaryDataWriter(ms))
			{
				binaryHandler.Serialize(buffer, layer.Data);
				ms.Position = 0;
				fileSource.Write(fileName, ms.ToArray());
			}
		}
	}
}

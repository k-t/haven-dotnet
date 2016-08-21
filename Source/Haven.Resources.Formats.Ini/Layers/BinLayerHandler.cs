using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haven.Resources.Formats.Binary;
using Haven.Utils;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class BinLayerHandler : ILayerHandler
	{
		internal const string Prefix = "bin/";

		private const string DataFileKey = "data";
		private static readonly string[] FileKeys = { DataFileKey };

		private readonly IBinaryLayerHandler binaryHandler;
		private readonly string sectionName;

		public Type DataType
		{
			get { return binaryHandler.LayerType; }
		}

		public IEnumerable<string> ExternalFileKeys
		{
			get { return FileKeys; }
		}

		public string SectionName
		{
			get { return sectionName; }
		}

		public BinLayerHandler(IBinaryLayerHandler binaryHandler)
		{
			this.binaryHandler = binaryHandler;
			this.sectionName = $"{Prefix}{binaryHandler.LayerName}";
		}

		public string GetExternalFileExtension(string externalFileKey, object data)
		{
			switch (externalFileKey)
			{
				case DataFileKey:
					return "." + binaryHandler.LayerName;
				default:
					throw new ArgumentException(
						$"Unknown external file key '{externalFileKey}'", nameof(externalFileKey));
			}
		}

		public object Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			using (var reader = new BinaryDataReader(context.LoadExternalFile(DataFileKey)))
				return binaryHandler.Deserialize(reader);
		}

		public void Save(IniKeyCollection iniData, object data, LayerHandlerContext context)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			using (var ms = new MemoryStream())
			using (var buffer = new BinaryDataWriter(ms))
			{
				binaryHandler.Serialize(buffer, data);
				ms.Position = 0;
				context.SaveExternalFile(DataFileKey, ms.ToArray());
			}
		}
	}
}

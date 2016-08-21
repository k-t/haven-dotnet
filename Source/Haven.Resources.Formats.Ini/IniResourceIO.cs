using System.Collections.Generic;
using System.IO;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini
{
	public static class IniResourceIO
	{
		private const string HeadSectionName = "res";

		private static readonly IniOptions IniOptions = new IniOptions {
			KeyDuplicate = IniDuplication.Allowed,
			SectionDuplicate = IniDuplication.Allowed,
			SectionNameCaseSensitive = false
		};

		private static readonly LayerHandlerProvider Handlers =
			new LayerHandlerProvider();

		public static void Load(this IniResource res, string path)
		{
			var fileSource = new FolderFileSource(Path.GetDirectoryName(path));
			using (var fs = File.OpenRead(path))
				res.Load(fs, fileSource);
		}

		public static void Load(this IniResource res, Stream stream, IFileSource fileSource)
		{
			var file = new IniFile(IniOptions);
			file.Load(stream);

			var header = file.Sections[HeadSectionName];
			if (header == null)
				throw new ResourceException("Header section is missing");

			res.Version = header.Keys.GetInt32("version", 1);
			res.Layers.Clear();
			foreach (var section in file.Sections)
			{
				var handler = Handlers.GetByName(section.Name);
				if (handler != null)
				{
					// get external file names to use them in the context
					var externalFiles = new Dictionary<string, string>();
					foreach (var key in handler.ExternalFileKeys)
						externalFiles[key] = section.Keys[key].Value;

					var context = new LayerHandlerContext(fileSource, externalFiles);
					var layerData = handler.Load(section.Keys, context);
					var layer = new IniLayer(layerData, externalFiles);
					res.Layers.Add(layer);
				}
			}
		}

		public static void Save(this IniResource res, string path)
		{
			var fileSource = new FolderFileSource(Path.GetDirectoryName(path));
			using (var fs = File.OpenWrite(path))
				res.Save(fs, fileSource);
		}

		public static void Save(this IniResource res, Stream stream, IFileSource fileSource)
		{
			var file = new IniFile(IniOptions);

			var header = file.Sections.Add(HeadSectionName);
			header.Keys.Add("version", res.Version);

			foreach (var layer in res.Layers)
			{
				var handler = Handlers.Get(layer.Data);
				if (handler != null)
				{
					var section = file.Sections.Add(handler.SectionName);
					var context = new LayerHandlerContext(fileSource, layer.ExternalFiles);
					handler.Save(section.Keys, layer.Data, context);

					// save external file names
					foreach (var key in handler.ExternalFileKeys)
						section.Keys.Add(key, layer.ExternalFiles[key]);
				}
			}

			file.Save(stream);
		}
	}
}

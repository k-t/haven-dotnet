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

		private static readonly IniLayerHandlerProvider Handlers =
			new IniLayerHandlerProvider();

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
					var layer = handler.Load(section.Keys, fileSource);
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
					handler.Save(layer, section.Keys, fileSource);
				}
			}

			file.Save(stream);
		}
	}
}

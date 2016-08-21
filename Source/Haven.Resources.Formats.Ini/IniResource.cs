using System.Collections.Generic;
using System.IO;
using System.Linq;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini
{
	public class IniResource
	{
		private const string HeadSectionName = "res";

		private static readonly LayerHandlerProvider Handlers = new LayerHandlerProvider();
		private static readonly IniOptions IniOptions = new IniOptions
		{
			KeyDuplicate = IniDuplication.Allowed,
			SectionDuplicate = IniDuplication.Allowed,
			SectionNameCaseSensitive = false
		};

		private int version;
		private readonly List<IniLayer> layers;

		public IniResource() : this(0)
		{
		}

		public IniResource(int version)
		{
			this.version = version;
			this.layers = new List<IniLayer>();
		}

		public ICollection<IniLayer> Layers
		{
			get { return layers; }
		}

		public int Version
		{
			get { return version; }
		}

		public void Load(string path)
		{
			var fileSource = new FolderFileSource(Path.GetDirectoryName(path));
			using (var fs = File.OpenRead(path))
				Load(fs, fileSource);
		}

		public void Load(Stream stream, IFileSource fileSource)
		{
			var file = new IniFile(IniOptions);
			file.Load(stream);

			var header = file.Sections[HeadSectionName];
			if (header == null)
				throw new ResourceException("Header section is missing");

			this.version = header.Keys.GetInt32("version", 1);
			this.layers.Clear();

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

					this.Layers.Add(layer);
				}
			}
		}

		public void Save(string path)
		{
			var fileSource = new FolderFileSource(Path.GetDirectoryName(path));
			using (var fs = File.OpenWrite(path))
				Save(fs, fileSource);
		}

		public void Save(Stream stream, IFileSource fileSource)
		{
			var file = new IniFile(IniOptions);

			var header = file.Sections.Add(HeadSectionName);
			header.Keys.Add("version", Version);

			foreach (var layer in Layers)
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

		public Resource ToResource()
		{
			return new Resource(Version, Layers.Select(x => x.Data));
		}

		public static IniResource FromResource(Resource res, string resName)
		{
			var result = new IniResource(res.Version);

			int i = 0;
			foreach (var data in res.GetLayers())
			{
				var handler = Handlers.Get(data);
				if (handler == null)
					continue;

				// generate file names for external files
				var externalFiles = new Dictionary<string, string>();
				foreach (var key in handler.ExternalFileKeys)
				{
					var ext = handler.GetExternalFileExtension(key, data);
					externalFiles[key] = $"{resName}_{i}{ext}";
				}

				result.Layers.Add(new IniLayer(data, externalFiles));
				i++;
			}

			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class CodeEntryHandler : GenericLayerHandler<CodeEntryLayer>
	{
		public CodeEntryHandler() : base("codeentry")
		{
		}

		protected override void Init(IniLayer layer, CodeEntryLayer data)
		{
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var entries = new List<CodeEntry>();
			var refs = new List<ResourceRef>();
			foreach (var key in keys)
			{
				switch (key.Name.ToLower())
				{
					case "entry":
					{
						var parts = key.Value.Split(':');
						if (parts.Length != 2)
							throw new FormatException("Invalid entry: " + key.Value);
						entries.Add(new CodeEntry(parts[0], parts[1]));
						break;
					}
					case "ref":
					{
						var parts = key.Value.Split(':');
						if (parts.Length != 2)
							throw new FormatException("Invalid ref: " + key.Value);
						refs.Add(new ResourceRef(parts[0], ushort.Parse(parts[1])));
						break;
					}
				}
			}
			var data = new CodeEntryLayer();
			data.Entries = entries.ToArray();
			data.Classpath = refs.ToArray();
			layer.Data = data;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (CodeEntryLayer)layer.Data;
			foreach (var entry in data.Entries)
				keys.Add("entry", $"{entry.Name}:{entry.ClassName}");
			foreach (var classpath in data.Classpath)
				keys.Add("ref", $"{classpath.Name}:{classpath.Version}");
		}
	}
}

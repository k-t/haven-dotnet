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

		protected override CodeEntryLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			var entries = new List<CodeEntry>();
			var refs = new List<ResourceRef>();

			foreach (var key in iniData)
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

			return new CodeEntryLayer
			{
				Entries = entries.ToArray(),
				Classpath = refs.ToArray()
			};
		}

		protected override void Save(IniKeyCollection iniData, CodeEntryLayer data, LayerHandlerContext context)
		{
			foreach (var entry in data.Entries)
				iniData.Add("entry", $"{entry.Name}:{entry.ClassName}");
			foreach (var classpath in data.Classpath)
				iniData.Add("ref", $"{classpath.Name}:{classpath.Version}");
		}
	}
}

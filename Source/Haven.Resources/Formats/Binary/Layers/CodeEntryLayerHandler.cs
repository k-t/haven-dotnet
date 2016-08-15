using System.Collections.Generic;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public class CodeEntryLayerHandler : GenericLayerHandler<CodeEntryLayer>
	{
		private const byte EntriesPart = 1;
		private const byte ClasspathPart = 2;

		public CodeEntryLayerHandler() : base("codeentry")
		{
		}

		protected override CodeEntryLayer Deserialize(BinaryDataReader reader)
		{
			var entries = new List<CodeEntry>();
			var classpath = new List<ResourceRef>();
			while (reader.HasRemaining)
			{
				var type = reader.ReadByte();
				switch (type)
				{
					case EntriesPart:
						while (true)
						{
							var name = reader.ReadCString();
							var className = reader.ReadCString();
							if (name.Length == 0)
								break;
							entries.Add(new CodeEntry(name, className));
						}
						break;
					case ClasspathPart:
						while (true)
						{
							var ln = reader.ReadCString();
							if (ln.Length == 0)
								break;
							var ver = reader.ReadUInt16();
							classpath.Add(new ResourceRef(ln, ver));
						}
						break;
				}
			}
			return new CodeEntryLayer {
				Entries = entries.ToArray(),
				Classpath = classpath.ToArray()
			};
		}

		protected override void Serialize(BinaryDataWriter writer, CodeEntryLayer data)
		{
			// entries
			if (data.Entries != null && data.Entries.Length > 0)
			{
				writer.Write(EntriesPart);
				foreach (var entry in data.Entries)
				{
					writer.WriteCString(entry.Name);
					writer.WriteCString(entry.ClassName);
				}
				writer.WriteCString("");
				writer.WriteCString("");
			}
			// classpath
			if (data.Classpath != null && data.Classpath.Length > 0)
			{
				writer.Write(ClasspathPart);
				foreach (var res in data.Classpath)
				{
					writer.WriteCString(res.Name);
					writer.Write(res.Version);
				}
				writer.WriteCString("");
			}
		}
	}
}

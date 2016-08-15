namespace Haven.Resources
{
	public class CodeEntryLayer
	{
		public CodeEntry[] Entries { get; set; }
		public ResourceRef[] Classpath { get; set; }
	}

	public struct CodeEntry
	{
		public CodeEntry(string name, string className) : this()
		{
			Name = name;
			ClassName = className;
		}

		public string Name { get; set; }
		public string ClassName { get; set; }
	}
}

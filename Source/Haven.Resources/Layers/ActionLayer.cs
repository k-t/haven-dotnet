namespace Haven.Resources
{
	public class ActionLayer
	{
		public string Name { get; set; }
		public string Prerequisite { get; set; }
		public ResourceRef Parent { get; set; }
		public char Hotkey { get; set; }
		public string[] Verbs { get; set; }
	}
}

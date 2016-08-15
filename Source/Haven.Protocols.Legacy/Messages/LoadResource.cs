namespace Haven.Protocols.Legacy.Messages
{
	public class LoadResource
	{
		public ushort ResourceId { get; set; }

		public string Name { get; set; }

		public ushort Version { get; set; }
	}
}
namespace Haven.Protocols.Hafen.Messages
{
	public class WidgetCreate
	{
		public ushort Id { get; set; }
		public ushort ParentId { get; set; }
		public string Type { get; set; }
		public object[] CArgs { get; set; }
		public object[] PArgs { get; set; }
	}
}

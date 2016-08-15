namespace Haven.Protocols.Legacy.Messages
{
	public class WidgetCreate
	{
		public ushort Id { get; set; }

		public ushort ParentId { get; set; }

		public string Type { get; set; }

		public Point2D Position { get; set; }

		public object[] Args { get; set; }
	}
}
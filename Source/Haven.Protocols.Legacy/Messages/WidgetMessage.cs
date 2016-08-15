namespace Haven.Protocols.Legacy.Messages
{
	public class WidgetMessage
	{
		public WidgetMessage()
		{
		}

		public WidgetMessage(ushort widgetId, string name, object[] args)
		{
			WidgetId = widgetId;
			Name = name;
			Args = args;
		}

		public ushort WidgetId { get; set; }

		public string Name { get; set; }

		public object[] Args { get; set; }
	}
}
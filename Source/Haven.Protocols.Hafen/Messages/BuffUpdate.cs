namespace Haven.Protocols.Hafen.Messages
{
	public class BuffUpdate
	{
		public int Id { get; set; }

		public int ResourceId { get; set; }

		public string Tooltip { get; set; }

		public int AMeter { get; set; }

		public int NMeter { get; set; }

		public int CMeter { get; set; }

		public int CTicks { get; set; }

		public long Time { get; set; }

		public bool IsMajor { get; set; }
	}
}
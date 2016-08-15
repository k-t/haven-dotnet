namespace Haven.Protocols.Hafen.Messages
{
	public class PartyUpdateMember
	{
		public int MemberId { get; set; }

		public Color Color { get; set; }

		public Point2D? Location { get; set; }
	}
}
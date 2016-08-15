namespace Haven.Protocols.Legacy.Messages
{
	public class PartyUpdateMember
	{
		public int MemberId { get; set; }

		public Color Color { get; set; }

		public Point2D? Location { get; set; }
	}
}
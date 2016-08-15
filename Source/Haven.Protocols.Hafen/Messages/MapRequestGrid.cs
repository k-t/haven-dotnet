namespace Haven.Protocols.Hafen.Messages
{
	public class MapRequestGrid
	{
		public MapRequestGrid()
		{
		}

		public MapRequestGrid(Point2D coord)
		{
			Coord = coord;
		}

		public Point2D Coord { get; set; }
	}
}

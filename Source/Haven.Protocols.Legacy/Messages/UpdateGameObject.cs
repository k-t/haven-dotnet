using System.Collections.Generic;

namespace Haven.Protocols.Legacy.Messages
{
	public class UpdateGameObject
	{
		private readonly List<GobDelta> deltas = new List<GobDelta>();

		public bool ReplaceFlag { get; set; }

		public int GobId { get; set; }

		public int Frame { get; set; }

		public IList<GobDelta> Deltas
		{
			get { return deltas; }
		}
	}

	public abstract class GobDelta
	{
		public class Clear : GobDelta
		{
		}

		public class Position : GobDelta
		{
			public Point2D Value { get; set; }
		}

		public class Resource : GobDelta
		{
			public int Id { get; set; }
			public byte[] SpriteData { get; set; }
		}

		public class StartMovement : GobDelta
		{
			public Point2D Origin { get; set; }
			public Point2D Destination { get; set; }
			public int TotalSteps { get; set; }
		}

		public class AdjustMovement : GobDelta
		{
			public int Step { get; set; }
		}

		public class Speech : GobDelta
		{
			public Point2D Offset { get; set; }
			public string Text { get; set; }
		}

		public class Layers : GobDelta
		{
			public int BaseResourceId { get; set; }
			public int[] ResourceIds { get; set; }
		}

		public class Avatar : GobDelta
		{
			public int[] ResourceIds { get; set; }
		}

		public class DrawOffset : GobDelta
		{
			public Point2D Value { get; set; }
		}

		public class Light : GobDelta
		{
			public Point2D Offset { get; set; }
			public int Size { get; set; }
			public byte Intensity { get; set; }
		}

		public class Follow : GobDelta
		{
			public int GobId { get; set; }
			public byte Szo { get; set; } // TODO: Rename
			public Point2D Offset { get; set; }
		}

		public class Homing : GobDelta
		{
			public int GobId { get; set; }
			public Point2D Target { get; set; }
			public int Velocity { get; set; }
		}

		public class Overlay : GobDelta
		{
			public int Id { get; set; }
			public bool IsPersistent { get; set; }
			public int ResourceId { get; set; }
			public byte[] SpriteData { get; set; }
		}

		public class Health : GobDelta
		{
			public byte Value { get; set; }
		}

		public class Buddy : GobDelta
		{
			public string Name { get; set; }
			public byte Group { get; set; }
			public byte Type { get; set; }
		}
	}
}
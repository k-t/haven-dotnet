using System;
using System.Collections.Generic;

namespace Haven.Protocols.Hafen.Messages
{
	public class UpdateGameObject
	{
		private readonly List<GobDelta> deltas = new List<GobDelta>();

		public UpdateGameObjectFlags Flags { get; set; }

		public long GobId { get; set; }

		public int Frame { get; set; }

		public IList<GobDelta> Deltas
		{
			get { return deltas; }
		}
	}

	[Flags]
	public enum UpdateGameObjectFlags : byte
	{
		Replace = 1,
		Virtual = 2
	}

	public abstract class GobDelta
	{
		public class Clear : GobDelta
		{
		}

		public class Position : GobDelta
		{
			public Point2D Coord { get; set; }
			public double Angle { get; set; }
		}

		public class Resource : GobDelta
		{
			public int Id { get; set; }
			public byte[] Data { get; set; }
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
			public float Offset { get; set; }
			public string Text { get; set; }
		}

		public class Composite : GobDelta
		{
			public int ResourceId { get; set; }
		}

		public class CompositePose : GobDelta
		{
			public int Seq { get; set; }
			public bool Interpolate { get; set; }
			public float Time { get; set; }
			public Resource[] Poses { get; set; }
			public Resource[] TPoses { get; set; }
		}

		public class CompositeModel : GobDelta
		{
		}

		public class CompositeEqu : GobDelta
		{
		}

		public class Avatar : GobDelta
		{
			public int[] ResourceIds { get; set; }
		}

		public class ZOffset : GobDelta
		{
			public float Value { get; set; }
		}

		public class Light : GobDelta
		{
			public Point2D Offset { get; set; }
			public int Size { get; set; }
			public byte Intensity { get; set; }
		}

		public class Follow : GobDelta
		{
			public long GobId { get; set; }
			public int ResId { get; internal set; }
			public string Name { get; set; }
		}

		public class Homing : GobDelta
		{
			public long GobId { get; set; }
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

		public class Icon : GobDelta
		{
			public int ResourceId { get; set; }
		}
	}
}
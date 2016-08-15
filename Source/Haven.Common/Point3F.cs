using System;

namespace Haven
{
	public struct Point3F : IEquatable<Point3F>
	{
		public static readonly Point3F Empty = new Point3F(0, 0, 0);

		public Point3F(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public float Z { get; set; }

		public override bool Equals(object obj)
		{
			return (obj is Point3F) && Equals((Point3F)obj);
		}

		public bool Equals(Point3F other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X.GetHashCode();
			hash = (hash * 7) + Y.GetHashCode();
			hash = (hash * 7) + Z.GetHashCode();
			return hash;
		}

		public static bool operator ==(Point3F a, Point3F b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Point3F a, Point3F b)
		{
			return !(a == b);
		}
	}
}

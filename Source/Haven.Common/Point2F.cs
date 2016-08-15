using System;

namespace Haven
{
	public struct Point2F : IEquatable<Point2F>
	{
		public static readonly Point2F Empty = new Point2F(0, 0);

		public Point2F(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Point2F(Point2F other)
		{
			X = other.X;
			Y = other.Y;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public Point2F Add(float x, float y)
		{
			return new Point2F(X + x, Y + y);
		}

		public Point2F Add(Point2F b)
		{
			return Add(b.X, b.Y);
		}

		public Point2F Add(float value)
		{
			return Add(value, value);
		}

		public Point2F Sub(float x, float y)
		{
			return new Point2F(X - x, Y - y);
		}

		public Point2F Sub(Point2F b)
		{
			return Sub(b.X, b.Y);
		}

		public Point2F Sub(float value)
		{
			return Sub(-value, -value);
		}

		public Point2F Mul(float x, float y)
		{
			return new Point2F(X * x, Y * y);
		}

		public Point2F Mul(Point2F b)
		{
			return Mul(b.X, b.Y);
		}

		public Point2F Mul(float value)
		{
			return Mul(value, value);
		}

		public double DistanceTo(Point2F other)
		{
			return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
		}

		public bool Equals(Point2F other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		public override bool Equals(object obj)
		{
			return (obj is Point2F) && Equals((Point2F)obj);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X.GetHashCode();
			hash = (hash * 7) + Y.GetHashCode();
			return hash;
		}

		public static Point2F operator +(Point2F a, Point2F b)
		{
			return a.Add(b.X, b.Y);
		}

		public static Point2F operator +(Point2F a, float value)
		{
			return a.Add(value);
		}

		public static Point2F operator -(Point2F a, Point2F b)
		{
			return a.Sub(b.X, b.Y);
		}

		public static Point2F operator -(Point2F a, float value)
		{
			return a.Sub(value);
		}

		public static Point2F operator *(Point2F a, Point2F b)
		{
			return a.Mul(b.X, b.Y);
		}

		public static Point2F operator *(Point2F a, float value)
		{
			return a.Mul(value);
		}

		public static bool operator ==(Point2F a, Point2F b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Point2F a, Point2F b)
		{
			return !(a == b);
		}
	}
}

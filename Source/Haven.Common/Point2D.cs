using System;

namespace Haven
{
	public struct Point2D : IEquatable<Point2D>
	{
		public static readonly Point2D Empty = new Point2D(0, 0);

		public Point2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point2D(Point2D other)
		{
			X = other.X;
			Y = other.Y;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public Point2D Add(int x, int y)
		{
			return new Point2D(X + x, Y + y);
		}

		public Point2D Add(Point2D b)
		{
			return Add(b.X, b.Y);
		}

		public Point2D Add(int value)
		{
			return Add(value, value);
		}

		public Point2D Sub(int x, int y)
		{
			return new Point2D(X - x, Y - y);
		}

		public Point2D Sub(Point2D b)
		{
			return Sub(b.X, b.Y);
		}

		public Point2D Sub(int value)
		{
			return Sub(-value, -value);
		}

		public Point2D Mul(int x, int y)
		{
			return new Point2D(X * x, Y * y);
		}

		public Point2D Mul(Point2D b)
		{
			return Mul(b.X, b.Y);
		}

		public Point2D Mul(int value)
		{
			return Mul(value, value);
		}

		public double DistanceTo(Point2D other)
		{
			return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
		}

		public bool Equals(Point2D other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return (obj is Point2D) && Equals((Point2D)obj);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X;
			hash = (hash * 7) + Y;
			return hash;
		}

		public static Point2D operator +(Point2D a, Point2D b)
		{
			return a.Add(b.X, b.Y);
		}

		public static Point2D operator +(Point2D a, int value)
		{
			return a.Add(value);
		}

		public static Point2D operator -(Point2D a, Point2D b)
		{
			return a.Sub(b.X, b.Y);
		}

		public static Point2D operator -(Point2D a, int value)
		{
			return a.Sub(value);
		}

		public static Point2D operator *(Point2D a, Point2D b)
		{
			return a.Mul(b.X, b.Y);
		}

		public static Point2D operator *(Point2D a, int value)
		{
			return a.Mul(value);
		}

		public static bool operator ==(Point2D a, Point2D b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Point2D a, Point2D b)
		{
			return !(a == b);
		}
	}
}

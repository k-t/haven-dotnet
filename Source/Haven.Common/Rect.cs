using System;

namespace Haven
{
	public struct Rect : IEquatable<Rect>
	{
		public static readonly Rect Empty = new Rect();

		public Rect(int x, int y, int width, int height) : this()
		{
			Location = new Point2D(x, y);
			Size = new Point2D(width, height);
		}

		public Point2D Location { get; set; }

		public Point2D Size { get; set; }

		public int X
		{
			get { return Location.X; }
		}

		public int Y
		{
			get { return Location.Y; }
		}

		public int Width
		{
			get { return Size.X; }
		}

		public int Height
		{
			get { return Size.Y; }
		}

		public int Top
		{
			get { return Y; }
		}

		public int Bottom
		{
			get { return Y + Height; }
		}

		public int Left
		{
			get { return X; }
		}

		public int Right
		{
			get { return X + Width; }
		}

		public static Rect FromLTRB(Point2D topLeft, Point2D bottomRight)
		{
			return FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}

		public static Rect FromLTRB(int left, int top, int right, int bottom)
		{
			// TODO: check top > bottom and left > right
			return new Rect(left, top, right - left, bottom - top);
		}

		public bool Contains(Point2D c)
		{
			return Contains(c.X, c.Y);
		}

		public bool Contains(int x, int y)
		{
			return (x >= Left && x <= Right) && (y >= Top && y <= Bottom);
		}

		public void Offset(int x, int y)
		{
			Location = Location.Add(x, y);
		}

		public override bool Equals(object obj)
		{
			return (obj is Rect) && Equals((Rect)obj);
		}

		public bool Equals(Rect other)
		{
			return Location == other.Location && Size == other.Size;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = (hash * 13) + Location.GetHashCode();
			hash = (hash * 13) + Size.GetHashCode();
			return hash;
		}

		public static bool operator ==(Rect a, Rect b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Rect a, Rect b)
		{
			return !(a == b);
		}
	}
}

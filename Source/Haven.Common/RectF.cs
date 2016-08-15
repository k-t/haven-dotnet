using System;

namespace Haven
{
	public struct RectF : IEquatable<RectF>
	{
		public RectF(float x, float y, float width, float height) : this()
		{
			Location = new Point2F(x, y);
			Size = new Point2F(width, height);
		}

		public Point2F Location { get; set; }

		public Point2F Size { get; set; }

		public float X
		{
			get { return Location.X; }
		}

		public float Y
		{
			get { return Location.Y; }
		}

		public float Width
		{
			get { return Size.X; }
		}

		public float Height
		{
			get { return Size.Y; }
		}

		public float Top
		{
			get { return Y; }
		}

		public float Bottom
		{
			get { return Y + Height; }
		}

		public float Left
		{
			get { return X; }
		}

		public float Right
		{
			get { return X + Width; }
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
			return (obj is RectF) && Equals((RectF)obj);
		}

		public bool Equals(RectF other)
		{
			return Location.Equals(other.Location) && Size.Equals(other.Size);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = (hash * 13) + Location.GetHashCode();
			hash = (hash * 13) + Size.GetHashCode();
			return hash;
		}

		public static RectF FromLTRB(Point2F topLeft, Point2F bottomRight)
		{
			return FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}

		public static RectF FromLTRB(float left, float top, float right, float bottom)
		{
			// TODO: check top > bottom and left > right
			return new RectF(left, top, right - left, bottom - top);
		}

		public static implicit operator RectF(Rect rect)
		{
			return new RectF(rect.X, rect.Y, rect.Width, rect.Height);
		}
	}
}

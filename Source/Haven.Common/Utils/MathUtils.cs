using System;

namespace Haven.Utils
{
	public static class MathUtils
	{
		public static int Div(this int n, int d)
		{
			var v = ((n < 0) ? (n + 1) : n) / d;
			if (n < 0)
				v--;
			return v;
		}

		public static int Mod(this int n, int d)
		{
			var v = n % d;
			if (v < 0)
				v += d;
			return v;
		}

		public static Point2D PolarToCartesian(double angle, double magnitude)
		{
			return new Point2D(
				(int)(Math.Cos(angle) * magnitude),
				(int)-(Math.Sin(angle) * magnitude));
		}
	}
}

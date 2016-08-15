using System;

namespace Haven
{
	public struct Color : IEquatable<Color>
	{
		public static readonly Color Transparent = FromArgb(0, 0, 0, 0);
		public static readonly Color White = FromArgb(255, 255, 255);
		public static readonly Color Black = FromArgb(0, 0, 0);
		public static readonly Color Red = FromArgb(255, 0, 0);
		public static readonly Color Green = FromArgb(0, 255, 0);
		public static readonly Color Blue = FromArgb(0, 0, 255);
		public static readonly Color Yellow = FromArgb(255, 255, 0);

		public byte A { get; set; }
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }

		public Color Contrast()
		{
			byte max = Math.Max(Math.Max(R, G), B);
			if (max > 128)
				return FromArgb(A, R / 2, G / 2, B / 2);
			if (max == 0)
				return White;
			var f = (byte)(128 / max);
			return FromArgb(A, R * f, G * f, B * f);
		}

		public bool Equals(Color other)
		{
			return (A == other.A) && (R == other.R) && (G == other.G) && (B == other.B);
		}

		public override bool Equals(object obj)
		{
			return (obj is Color) && Equals((Color)obj);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + A;
			hash = (hash * 7) + R;
			hash = (hash * 7) + G;
			hash = (hash * 7) + B;
			return hash;
		}

		public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
		{
			return new Color { A = alpha, B = blue, R = red, G = green };
		}

		public static Color FromArgb(int alpha, int red, int green, int blue)
		{
			return FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue);
		}

		public static Color FromArgb(byte red, byte green, byte blue)
		{
			return FromArgb((byte)255, red, green, blue);
		}

		public static Color FromArgb(int red, int green, int blue)
		{
			return FromArgb((byte)red, (byte)green, (byte)blue);
		}

		public static Color FromArgb(byte alpha, Color color)
		{
			return new Color { A = alpha, B = color.B, R = color.R, G = color.G };
		}

		public static bool operator ==(Color a, Color b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Color a, Color b)
		{
			return !(a == b);
		}
	}
}

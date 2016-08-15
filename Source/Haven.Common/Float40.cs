using System;

namespace Haven
{
	public struct Float40
	{
		private sbyte exp;
		private double mantissa;
		private bool negative;

		public Float40(double value)
		{
			if (Math.Abs(value) >= double.Epsilon)
			{ 
				exp = (sbyte)Math.Floor(Math.Log(Math.Abs(value)) / Math.Log(2));
				mantissa = (Math.Abs(value / Math.Pow(2, exp)) - 1) * 2147483648;
				negative = (value < 0);
			}
			else
			{
				exp = -128;
				mantissa = 0;
				negative = false;
			}
		}

		public static explicit operator double(Float40 n)
		{
			return n.ToDouble();
		}

		public static byte[] Encode(Float40 n)
		{
			var bytes = new byte[5];
			bytes[0] = (byte)n.exp;
			Array.Copy(BitConverter.GetBytes((uint)n.mantissa), 0, bytes, 1, 4);
			if (n.negative)
				bytes[4] |= 128;
			return bytes;
		}

		public static Float40 Decode(byte[] bytes)
		{
			if (bytes.Length < 5)
				throw new ArgumentException("Source array is not long enough", nameof(bytes));
			var n = new Float40();
			n.exp = (sbyte)bytes[0];
			long t = BitConverter.ToUInt32(bytes, 1);
			n.mantissa = (t & 0x7fffffffL);
			n.negative = (t & 0x80000000L) != 0;
			return n;
		}

		public double ToDouble()
		{
			if (exp == -128)
				return 0.0;
			var value = (mantissa / 2147483648.0) + 1.0;
			if (negative)
				value = -value;
			return (Math.Pow(2.0, exp) * value);
		}

		public override string ToString()
		{
			return ToDouble().ToString();
		}
	}
}

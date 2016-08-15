using System.IO;
using System.Text;
using Haven.Utils;
using MiscUtil.Conversion;

namespace Haven.Net
{
	public class BinaryMessageWriter
	{
		private readonly byte type;
		private readonly MemoryStream stream;
		private readonly BinaryDataWriter writer;

		public BinaryMessageWriter(byte type)
		{
			this.type = type;
			this.stream = new MemoryStream();
			this.writer = new BinaryDataWriter(stream);
		}

		public BinaryMessageWriter Bytes(byte[] src, int off, int len)
		{
			writer.Write(src, off, len);
			return this;
		}

		public BinaryMessageWriter Bytes(byte[] src)
		{
			Bytes(src, 0, src.Length);
			return this;
		}

		public BinaryMessageWriter Byte(byte value)
		{
			writer.Write(value);
			return this;
		}

		public BinaryMessageWriter UInt16(ushort value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter UInt32(uint value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter Int32(int value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter String(string str)
		{
			Chars(str);
			Bytes(new byte[] { 0 });
			return this;
		}

		public BinaryMessageWriter Chars(char[] chars)
		{
			Bytes(Encoding.UTF8.GetBytes(chars));
			return this;
		}

		public BinaryMessageWriter Chars(string str)
		{
			Bytes(Encoding.UTF8.GetBytes(str));
			return this;
		}

		public BinaryMessageWriter Coord(int x, int y)
		{
			Int32(x);
			Int32(y);
			return this;
		}

		public BinaryMessageWriter Coord(Point2D c)
		{
			return Coord(c.X, c.Y);
		}

		public BinaryMessageWriter List(params object[] args)
		{
			writer.WriteList(args);
			return this;
		}

		public BinaryMessage Complete()
		{
			return new BinaryMessage(type, stream.ToArray());
		}
	}
}

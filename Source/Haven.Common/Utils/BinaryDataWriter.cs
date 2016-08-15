using System;
using System.IO;

namespace Haven.Utils
{
	public class BinaryDataWriter : IDisposable
	{
		private readonly Stream stream;
		private readonly BinaryWriter writer;

		public BinaryDataWriter(Stream stream)
		{
			this.stream = stream;
			this.writer = new BinaryWriter(stream);
		}

		public void Dispose()
		{
			if (stream != null)
				stream.Dispose();
		}

		public void Write(byte value)
		{
			writer.Write(value);
		}

		public void Write(byte[] value)
		{
			writer.Write(value);
		}

		public void Write(byte[] value, int index, int count)
		{
			writer.Write(value, index, count);
		}

		public void Write(bool value)
		{
			writer.Write(value);
		}

		public void Write(char value)
		{
			writer.Write(value);
		}

		public void Write(char[] value)
		{
			writer.Write(value);
		}

		public void Write(short value)
		{
			writer.Write(value);
		}

		public void Write(int value)
		{
			writer.Write(value);
		}

		public void Write(long value)
		{
			writer.Write(value);
		}

		public void Write(ushort value)
		{
			writer.Write(value);
		}

		public void Write(uint value)
		{
			writer.Write(value);
		}

		public void Write(ulong value)
		{
			writer.Write(value);
		}

		public void Write(float value)
		{
			writer.Write(value);
		}

		public void Write(double value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a C-style (null-terminated) string to the current stream.
		/// </summary>
		public void WriteCString(string value)
		{
			Write(value.ToCharArray());
			Write('\0');
		}

		/// <summary>
		/// Writes a point to the current stream.
		/// </summary>
		public void WriteInt16Coord(int x, int y)
		{
			Write((short)x);
			Write((short)y);
		}

		/// <summary>
		/// Writes a point to the current stream.
		/// </summary>
		public void WriteInt16Coord(Point2D value)
		{
			WriteInt16Coord(value.X, value.Y);
		}

		public void WriteInt32Coord(Point2D value)
		{
			Write(value.X);
			Write(value.Y);
		}

		public void WriteColor(Color color)
		{
			Write(color.R);
			Write(color.G);
			Write(color.B);
			Write(color.A);
		}

		public void WriteList(object[] list)
		{
			foreach (var item in list)
			{
				var type = BinaryListTypes.TypeOf(item);
				writer.Write((byte)type);
				switch (type)
				{
					case BinaryListType.Int32:
						writer.Write((int)item);
						break;
					case BinaryListType.String:
						WriteCString((string)item);
						break;
					case BinaryListType.Coord:
						WriteInt32Coord((Point2D)item);
						break;
					case BinaryListType.Byte:
						Write((byte)item);
						break;
					case BinaryListType.UInt16:
						writer.Write((ushort)item);
						break;
					case BinaryListType.Color:
						WriteColor((Color)item);
						break;
					case BinaryListType.List:
						WriteList((object[])item);
						break;
					case BinaryListType.SByte:
						writer.Write((sbyte)item);
						break;
					case BinaryListType.Int16:
						writer.Write((short)item);
						break;
					case BinaryListType.Bytes:
						var bytes = (byte[])item;
						if (bytes.Length < 128)
							writer.Write((byte)bytes.Length);
						else
						{
							writer.Write((byte)128);
							writer.Write(bytes.Length);
						}
						writer.Write(bytes);
						break;
					case BinaryListType.Single:
						writer.Write((float)item);
						break;
					case BinaryListType.Double:
						writer.Write((double)item);
						break;
					case BinaryListType.Uid:
						writer.Write((long)item);
						break;
				}
			}
			writer.Write((byte)BinaryListType.End);
		}

		public void WriteFloat40(double value)
		{
			writer.Write(Float40.Encode(new Float40(value)));
		}
	}
}

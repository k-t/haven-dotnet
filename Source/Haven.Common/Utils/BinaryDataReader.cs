using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Haven.Utils
{
	public class BinaryDataReader : IDisposable
	{
		private readonly Stream stream;
		private readonly BinaryReader reader;
		private readonly long? length;
		private readonly long initialPosition;

		public BinaryDataReader(Stream stream, long? length)
		{
			this.stream = stream;
			this.reader = new BinaryReader(stream);
			this.length = length;
			this.initialPosition = stream.Position;
		}

		public BinaryDataReader(Stream stream)
			: this(stream, stream.Length - stream.Position)
		{
		}

		public BinaryDataReader(byte[] bytes)
			: this(bytes, 0, bytes.Length)
		{
		}

		public BinaryDataReader(byte[] bytes, int offset, int length)
			: this(new MemoryStream(bytes, offset, length))
		{
		}

		public BinaryDataReader(BinaryDataReader buffer, int length)
			: this(buffer.stream, length)
		{
		}

		public long Length
		{
			get { return length ?? stream.Length; }
		}

		public long Remaining
		{
			get { return (initialPosition + Length) - stream.Position; }
		}

		public bool HasRemaining
		{
			get { return Remaining > 0; }
		}

		public long Position
		{
			get { return stream.Position; }
			set { stream.Position = value; }
		}

		public void Dispose()
		{
			if (stream != null)
				stream.Dispose();
		}

		public byte ReadByte()
		{
			return reader.ReadByte();
		}

		public sbyte ReadSByte()
		{
			return reader.ReadSByte();
		}

		public byte[] ReadBytes(int count)
		{
			return reader.ReadBytes(count);
		}

		public bool ReadBoolean()
		{
			return reader.ReadBoolean();
		}

		public char ReadChar()
		{
			return reader.ReadChar();
		}

		public char[] ReadChars(int count)
		{
			return reader.ReadChars(count);
		}

		public short ReadInt16()
		{
			return reader.ReadInt16();
		}

		public int ReadInt32()
		{
			return reader.ReadInt32();
		}

		public long ReadInt64()
		{
			return reader.ReadInt64();
		}

		public ushort ReadUInt16()
		{
			return reader.ReadUInt16();
		}

		public uint ReadUInt32()
		{
			return reader.ReadUInt32();
		}

		public ulong ReadUInt64()
		{
			return reader.ReadUInt64();
		}

		public float ReadSingle()
		{
			return reader.ReadSingle();
		}

		public double ReadDouble()
		{
			return reader.ReadDouble();
		}

		public byte[] ReadRemaining()
		{
			return ReadBytes((int)Remaining);
		}

		/// <summary>
		/// Reads a C-style (null-terminated) string from the current stream.
		/// </summary>
		public string ReadCString()
		{
			var bytes = new List<byte>();
			while (true)
			{
				if (HasRemaining)
				{
					var next = ReadByte();
					if (next == 0)
						break;
					bytes.Add(next);
				}
				else
				{
					if (bytes.Count != 0)
					{
						var str = Encoding.UTF8.GetString(bytes.ToArray());
						throw new Exception("Incomplete string at " + str);
					}
					return string.Empty;
				}

			}
			return Encoding.UTF8.GetString(bytes.ToArray());
		}

		public Point2D ReadInt16Coord()
		{
			return new Point2D(ReadInt16(), ReadInt16());
		}

		public Point2D ReadInt32Coord()
		{
			return new Point2D(ReadInt32(), ReadInt32());
		}

		public Color ReadColor()
		{
			var r = ReadByte();
			var g = ReadByte();
			var b = ReadByte();
			var a = ReadByte();
			return Color.FromArgb(a, r, g, b);
		}

		public double ReadFloat40()
		{
			return Float40.Decode(ReadBytes(5)).ToDouble();
		}

		public object[] ReadList()
		{
			var list = new List<object>();
			while (true)
			{
				var type = HasRemaining
					? (BinaryListType)ReadByte()
					: BinaryListType.End;
				switch (type)
				{
					case BinaryListType.End:
						return list.ToArray();
					case BinaryListType.Int32:
						list.Add(ReadInt32());
						break;
					case BinaryListType.String:
						list.Add(ReadCString());
						break;
					case BinaryListType.Coord:
						list.Add(ReadInt32Coord());
						break;
					case BinaryListType.Byte:
						list.Add(ReadByte());
						break;
					case BinaryListType.UInt16:
						list.Add(ReadUInt16());
						break;
					case BinaryListType.Color:
						list.Add(ReadColor());
						break;
					case BinaryListType.List:
						list.Add(ReadList());
						break;
					case BinaryListType.SByte:
						list.Add(ReadSByte());
						break;
					case BinaryListType.Int16:
						list.Add(ReadInt16());
						break;
					case BinaryListType.Nil:
						list.Add(null);
						break;
					case BinaryListType.Bytes:
						int length = ReadByte();
						if ((length & 128) != 0)
							length = ReadInt32();
						list.Add(ReadBytes(length));
						break;
					case BinaryListType.Single:
						list.Add(ReadSingle());
						break;
					case BinaryListType.Double:
						list.Add(ReadDouble());
						break;
					case BinaryListType.Uid:
						list.Add(ReadInt64());
						break;
					default:
						throw new Exception($"Encountered unknown type {type} in TTO list.");
				}
			}
		}
	}
}

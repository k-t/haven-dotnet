using System;
using Haven.Utils;

namespace Haven.Net
{
	public class BinaryMessage
	{
		private readonly byte type;
		private readonly byte[] buffer;
		private readonly int bufferOffset;
		private readonly int length;

		public BinaryMessage(byte type, byte[] buffer)
			: this(type, buffer, 0, buffer.Length)
		{
		}

		public BinaryMessage(byte type, byte[] buffer, int bufferOffset, int length)
		{
			this.type = type;
			this.buffer = buffer;
			this.bufferOffset = bufferOffset;
			this.length = length;
		}

		public int Length
		{
			get { return buffer.Length; }
		}

		public byte Type
		{
			get { return type; }
		}

		public byte[] GetData()
		{
			// if buffer is used directly, return it
			if (bufferOffset == 0 && length == buffer.Length)
				return buffer;

			var data = new byte[length];
			Array.Copy(buffer, bufferOffset, data, 0, length);
			return data;
		}

		public BinaryDataReader GetReader()
		{
			return new BinaryDataReader(buffer, bufferOffset, length);
		}

		public static BinaryMessageWriter Make(byte type)
		{
			return new BinaryMessageWriter(type);
		}
	}
}


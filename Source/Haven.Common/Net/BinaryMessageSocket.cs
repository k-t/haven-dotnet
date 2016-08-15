using System;
using System.Net;
using System.Net.Sockets;

namespace Haven.Net
{
	public class BinaryMessageSocket : IDisposable
	{
		private readonly Socket socket;
		private readonly byte[] receiveBuffer;
		private int receiveTimeout;

		public BinaryMessageSocket()
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			receiveBuffer = new byte[socket.ReceiveBufferSize];
		}

		public void Dispose()
		{
			socket.Close();
		}

		public void Connect(NetworkAddress address)
		{
			var endPoint = new IPEndPoint(Dns.GetHostEntry(address.Host).AddressList[0], address.Port);
			socket.Connect(endPoint);
		}

		public void Close()
		{
			socket.Close();
		}

		public bool Receive(out BinaryMessage message)
		{
			if (!socket.Poll(receiveTimeout * 1000, SelectMode.SelectRead))
			{
				message = null;
				return false;
			}

			int size = socket.Receive(receiveBuffer);
			if (size == 0)
				throw new Exception("Couldn't receive data from socket");
			var type = receiveBuffer[0];
			var buffer = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, buffer, 0, size - 1);

			message = new BinaryMessage(type, buffer);
			return true;
		}

		public void Send(BinaryMessage message)
		{
			var buffer = new byte[message.Length + 1];
			buffer[0] = message.Type;
			Array.Copy(message.GetData(), 0, buffer, 1, message.Length);
			socket.Send(buffer);
		}

		public void SetReceiveTimeout(int milliseconds)
		{
			receiveTimeout = milliseconds * 1000;
		}
	}
}

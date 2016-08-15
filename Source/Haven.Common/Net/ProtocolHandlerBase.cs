using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Haven.Messages;
using Haven.Messaging;
using Haven.Utils;

namespace Haven.Net
{
	public abstract class ProtocolHandlerBase : IProtocolHandler
	{
		#region Constants

		private const int ReceiveTimeout = 1000; // milliseconds

		internal const int MSG_SESS = 0;
		internal const int MSG_REL = 1;
		internal const int MSG_ACK = 2;
		internal const int MSG_BEAT = 3;
		internal const int MSG_MAPDATA = 5;
		internal const int MSG_OBJDATA = 6;
		internal const int MSG_OBJACK = 7;
		internal const int MSG_CLOSE = 8;

		#endregion

		private enum State
		{
			Closed = 0,
			Created = 1,
			Started = 2,
			Closing = 3,
		}

		private readonly BinaryMessageSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private readonly object stateLock = new object();
		private readonly Dictionary<ushort, BinaryMessage> waiting;
		private IMessageDispatcher dispatcher = NullMessageDispatcher.Instance;
		private ushort rseq;
		private State state;

		public ProtocolHandlerBase()
		{
			this.waiting = new Dictionary<ushort, BinaryMessage>();

			this.state = State.Created;
			this.socket = new BinaryMessageSocket();
			this.socket.SetReceiveTimeout(ReceiveTimeout);
			this.sender = new MessageSender(socket);
			this.receiver = new MessageReceiver(socket, ReceiveMessage);

			MonitorTask(sender);
			MonitorTask(receiver);
		}

		public virtual IMessageDispatcher Dispatcher
		{
			get { return dispatcher; }
			set { dispatcher = value ?? NullMessageDispatcher.Instance; }
		}

		public void Connect(NetworkAddress address, string userName, byte[] cookie)
		{
			try
			{
				lock (stateLock)
				{
					if (state != State.Created)
						throw new InvalidOperationException("Can't open already opened/closed connection");

					socket.Connect(address);
					DoHandshake(userName, cookie);
					receiver.Run();
					sender.Run();

					state = State.Started;
				}
			}
			catch (SocketException ex)
			{
				throw new NetworkException(ex.Message, ex);
			}
		}

		public void Close()
		{
			lock (stateLock)
			{
				if (state != State.Started)
					return;

				state = State.Closing;

				DemonitorTask(receiver);
				receiver.Stop();

				DemonitorTask(receiver);
				sender.Stop();

				socket.Send(BinaryMessage.Make(MSG_CLOSE).Complete());
				// HACK: otherwise close message may not be sent
				Thread.Sleep(TimeSpan.FromMilliseconds(5));
				socket.Close();

				state = State.Closed;
			}
			Dispatcher.Dispatch(new ExitMessage());
		}

		public virtual void Send<TMessage>(TMessage message)
		{
		}

		protected abstract BinaryMessage GetHelloMessage(string sessionId, byte[] cookie);
		protected abstract void HandleSeqMessage(BinaryMessage message);
		protected abstract void HandleGobData(BinaryDataReader reader);
		protected abstract void HandleMapData(BinaryDataReader reader);

		protected void AckGobData(long gobId, int frame)
		{
			var message = BinaryMessage.Make(MSG_OBJACK)
				.UInt32((uint)gobId)
				.Int32(frame)
				.Complete();
			// FIXME: make it smarter
			socket.Send(message);
		}

		protected void Receive<TMessage>(TMessage message)
		{
			dispatcher.Dispatch(message);
		}

		protected void Send(BinaryMessage message)
		{
			socket.Send(message);
		}

		protected void SendSeqMessage(BinaryMessage message)
		{
			sender.SendSeqMessage(message);
		}

		private void DoHandshake(string userName, byte[] cookie)
		{
			var hello = GetHelloMessage(userName, cookie);
			Send(hello);

			BinaryMessage reply;
			ConnectionError error;
			while (true)
			{
				if (!socket.Receive(out reply))
					throw new NetworkException(ConnectionError.ConnectionError);

				if (reply.Type == MSG_SESS)
				{
					error = (ConnectionError)reply.GetReader().ReadByte();
					break;
				}
				if (reply.Type == MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new NetworkException(error);
		}

		private void ReceiveMessage(BinaryMessage msg)
		{
			var reader = msg.GetReader();
			switch (msg.Type)
			{
				case MSG_REL:
					var seq = reader.ReadUInt16();
					while (reader.HasRemaining)
					{
						var type = reader.ReadByte();
						int len;
						if ((type & 0x80) != 0) // is not last?
						{
							type &= 0x7f;
							len = reader.ReadUInt16();
						}
						else
						{
							len = (int)reader.Remaining;
						}
						ReceiveSeqMessage(seq, new BinaryMessage(type, reader.ReadBytes(len)));
						seq++;
					}
					break;
				case MSG_ACK:
					sender.ReceiveAck(reader.ReadUInt16());
					break;
				case MSG_MAPDATA:
					HandleMapData(msg.GetReader());
					break;
				case MSG_OBJDATA:
					HandleGobData(msg.GetReader());
					break;
				case MSG_CLOSE:
					Close();
					return;
			}
		}

		private void ReceiveSeqMessage(ushort seq, BinaryMessage msg)
		{
			if (seq == rseq)
			{
				HandleSeqMessage(msg);
				while (true)
				{
					rseq++;
					if (!waiting.Remove(rseq, out msg))
						break;
					HandleSeqMessage(msg);
				}
				sender.SendAck((ushort)(rseq - 1));
			}
			else if (seq > rseq)
				waiting[seq] = msg;
		}

		private void MonitorTask(BackgroundTask task)
		{
			task.Finished += OnTaskFinished;
			task.Crashed += OnTaskCrashed;
		}

		private void DemonitorTask(BackgroundTask task)
		{
			task.Finished -= OnTaskFinished;
			task.Crashed -= OnTaskCrashed;
		}

		private void OnTaskFinished()
		{
			if (state != State.Closing && state != State.Closed)
				Close();
		}

		private void OnTaskCrashed(Exception ex)
		{
			if (state != State.Closing && state != State.Closed)
			{
				Dispatcher.Dispatch(new ExceptionMessage(ex));
				Close();
			}
		}
	}
}

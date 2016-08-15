using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Haven.Net;
using Haven.Utils;

namespace Haven.Protocols.Hafen
{
	public class HafenAuthHandler : IAuthHandler
	{
		private const string CmdTryPassword = "pw";
		private const string CmdTryToken = "token";
		private const string CmdGetCookie = "cookie";
		private const string CmdGetToken = "mktoken";
		private const string ReplyOk = "ok";
		private const string ReplyNo = "no";

		private SslStream ctx;

		private static bool ValidateServerCertificate(
			object sender,
			X509Certificate certificate,
			X509Chain chain,
			SslPolicyErrors sslPolicyErrors)
		{
			// TODO: proper certificate validation
			return true;
		}

		public void Connect(NetworkAddress address)
		{
			var tc = new TcpClient(address.Host, address.Port);
			ctx = new SslStream(tc.GetStream(), false, ValidateServerCertificate, null);
			ctx.AuthenticateAsClient(address.Host);
		}

		public AuthResult TryPassword(string userName, string password)
		{
			byte[] phash = Digest(password);
			var reply = Call(CmdTryPassword, userName, phash);
			var result = reply.ReadCString();
			switch (result)
			{
				case ReplyOk:
					var sessionId = reply.ReadCString();
					var sessionCookie = GetCookie();
					return AuthResult.Success(sessionId, sessionCookie);
				case ReplyNo:
					var error = reply.ReadCString();
					throw new AuthException(error);
				default:
					throw new AuthException($"Unexpected reply {result} from auth server");
			}
		}

		public AuthResult TryToken(string userName, byte[] token)
		{
			var reply = Call(CmdTryToken, userName, token);
			var result = reply.ReadCString();
			switch (result)
			{
				case ReplyOk:
					var sessionId = reply.ReadCString();
					var sessionCookie = GetCookie();
					return AuthResult.Success(sessionId, sessionCookie);
				case ReplyNo:
					return AuthResult.Fail();
				default:
					throw new AuthException($"Unexpected reply {result} from auth server");
			}
		}

		public byte[] GetToken()
		{
			var reply = Call(CmdGetToken);
			var result = reply.ReadCString();
			switch (result)
			{
				case ReplyOk:
					return reply.ReadBytes(32);
				default:
					throw new AuthException($"Unexpected reply {result} from auth server");
			}
		}

		private byte[] GetCookie()
		{
			var reply = Call(CmdGetCookie);
			var result = reply.ReadCString();
			switch (result)
			{
				case ReplyOk:
					return reply.ReadBytes(32);
				default:
					throw new AuthException($"Unexpected reply {result} from auth server");
			}
		}

		private static byte[] Digest(string password)
		{
			byte[] buf = Encoding.UTF8.GetBytes(password);
			SHA256 shaM = new SHA256Managed();
			return shaM.ComputeHash(buf);
		}

		private void ReadAll(byte[] buf)
		{
			int rv;
			for (int i = 0; i < buf.Length; i += rv)
			{
				rv = ctx.Read(buf, i, buf.Length - i);
				if (rv < 0)
					throw new AuthException("Premature end of input");
			}
		}

		private BinaryDataReader GetReply()
		{
			byte[] header = new byte[2];
			ReadAll(header);
			var length = (header[0] << 8) | header[1];
			byte[] buf = new byte[length];
			ReadAll(buf);
			return new BinaryDataReader(buf);
		}

		private BinaryDataReader Call(string cmd, params object[] args)
		{
			var msg = BinaryMessage.Make(0).String(cmd);
			foreach (var arg in args)
			{
				if (arg is string)
					msg.String((string)arg);
				else if (arg is byte[])
					msg.Bytes((byte[])arg);
				else
					throw new ArgumentException("Illegal argument: " + arg.GetType().FullName);
			}
			Send(msg.Complete());
			return GetReply();
		}

		private void Send(BinaryMessage msg)
		{
			if (msg.Length > 255)
				throw new AuthException("Message is too long (" + msg.Length + " bytes)");
			var bytes = new byte[msg.Length + 2];
			bytes[0] = (byte)((msg.Length & 0xff00) >> 8);
			bytes[1] = (byte)(msg.Length & 0x00ff);
			Array.Copy(msg.GetData(), 0, bytes, 2, msg.Length);
			ctx.Write(bytes);
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (ctx != null)
				ctx.Dispose();
		}

		#endregion
	}
}


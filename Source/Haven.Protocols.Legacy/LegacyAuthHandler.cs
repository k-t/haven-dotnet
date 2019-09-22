using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Haven.Net;

namespace Haven.Protocols.Legacy
{
	public class LegacyAuthHandler : IAuthHandler
	{
		private const int CMD_USR = 1;
		private const int CMD_PASSWD = 2;
		private const int CMD_GETTOKEN = 3;
		private const int CMD_USETOKEN = 4;

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
			ctx.AuthenticateAsClient(address.Host, new X509CertificateCollection(), SslProtocols.Default, false);
		}

		public AuthResult TryToken(string userName, byte[] token)
		{
			BindUser(userName);
			Send(BinaryMessage.Make(CMD_USETOKEN).Bytes(token).Complete());
			var reply = GetReply();
			if (reply.Type != 0)
			{
				return AuthResult.Fail();
			}
			var cookie = reply.GetData();
			return AuthResult.Success(userName, cookie);
		}

		public AuthResult TryPassword(string userName, string password)
		{
			BindUser(userName);
			byte[] phash = Digest(password);
			Send(BinaryMessage.Make(CMD_PASSWD).Bytes(phash).Complete());
			var reply = GetReply();
			if (reply.Type != 0)
			{
				return AuthResult.Fail();
			}
			var cookie = reply.GetData();
			return AuthResult.Success(userName, cookie);
		}

		public byte[] GetToken()
		{
			Send(BinaryMessage.Make(CMD_GETTOKEN).Complete());
			var reply = GetReply();
			return reply.Type == 0 ? reply.GetData() : null;
		}

		private void BindUser(string userName)
		{
			var msg = BinaryMessage.Make(CMD_USR).Chars(userName).Complete();
			Send(msg);
			var reply = GetReply();
			if (reply.Type != 0)
				throw new AuthException("Unhandled reply " + reply.Type + " when binding username");
		}

		private byte[] Digest(string password)
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

		private BinaryMessage GetReply()
		{
			byte[] header = new byte[2];
			ReadAll(header);
			byte[] buf = new byte[header[1]];
			ReadAll(buf);
			return new BinaryMessage(header[0], buf);
		}

		private void Send(BinaryMessage message)
		{
			if (message.Length > 255)
				throw new AuthException("Message is too long (" + message.Length + " bytes)");

			var bytes = new byte[message.Length + 2];
			bytes[0] = message.Type;
			bytes[1] = (byte)message.Length;

			Array.Copy(message.GetData(), 0, bytes, 2, message.Length);

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


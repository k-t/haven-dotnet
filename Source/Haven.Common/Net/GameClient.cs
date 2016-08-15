using System;
using Haven.Messaging;

namespace Haven.Net
{
	public class GameClient
	{
		private readonly GameClientConfig config;
		private readonly IAuthHandlerFactory authHandlerFactory;
		private readonly IProtocolHandlerFactory protocolHandlerFactory;
		private readonly MessageBroker messageBroker;
		private IProtocolHandler protocolHandler;
		private GameClientState state;
		private string userName;
		private string sessionId;
		private byte[] sessionCookie;

		public GameClient(
			GameClientConfig config,
			IAuthHandlerFactory authHandlerFactory,
			IProtocolHandlerFactory protocolHandlerFactory)
		{
			this.config = config;
			this.authHandlerFactory = authHandlerFactory;
			this.protocolHandlerFactory = protocolHandlerFactory;
			this.messageBroker = new MessageBroker();
			this.state = GameClientState.Initial;
		}

		public IMessageSource Messages
		{
			get { return messageBroker; }
		}

		public GameClientState State
		{
			get { return state; }
		}

		public string UserName
		{
			get { return userName; }
		}

		public AuthResult Authenticate(string userName, string password, bool requestToken)
		{
			CheckNotConnected();

			using (var authHandler = authHandlerFactory.Create())
			{
				authHandler.Connect(config.AuthServerAddress);

				var result = authHandler.TryPassword(userName, password);
				if (result.IsSuccessful)
				{
					this.userName = userName;
					this.sessionId = result.SessionId;
					this.sessionCookie = result.SessionCookie;
					this.state = GameClientState.Authenticated;
					var token = requestToken ? authHandler.GetToken() : null;
					return new AuthResult(true, result.SessionId, result.SessionCookie, token);
				}
				return result;
			}
		}

		public AuthResult Authenticate(string userName, byte[] token)
		{
			CheckNotConnected();

			using (var authHandler = authHandlerFactory.Create())
			{
				authHandler.Connect(config.AuthServerAddress);

				var result = authHandler.TryToken(userName, token);
				if (result.IsSuccessful)
				{
					this.userName = userName;
					this.sessionId = result.SessionId;
					this.sessionCookie = result.SessionCookie;
					this.state = GameClientState.Authenticated;
				}
				return result;
			}
		}

		public void Connect()
		{
			CheckAuthenticated();
			CheckNotConnected();

			protocolHandler = protocolHandlerFactory.Create();
			protocolHandler.Dispatcher = messageBroker;
			protocolHandler.Connect(config.GameServerAddress, sessionId, sessionCookie);

			this.state = GameClientState.Connected;
		}

		public void Close()
		{
			protocolHandler.Close();
			this.state = GameClientState.Initial;
		}

		public void Send<TMessage>(TMessage message)
		{
			CheckConnected();
			protocolHandler.Send(message);
		}

		private void CheckAuthenticated()
		{
			if (state != GameClientState.Authenticated)
				throw new InvalidOperationException("Client is not authenticated");
		}

		private void CheckConnected()
		{
			if (state != GameClientState.Connected)
				throw new InvalidOperationException("Client is not connected");
		}

		private void CheckNotConnected()
		{
			if (state == GameClientState.Connected)
				throw new InvalidOperationException("Client is currently connected");
		}
	}
}

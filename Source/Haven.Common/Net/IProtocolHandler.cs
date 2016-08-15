using System;
using Haven.Messaging;

namespace Haven.Net
{
	/// <summary>
	/// Interface for game protocol implementations.
	/// </summary>
	public interface IProtocolHandler
	{
		/// <summary>
		/// Gets or sets message dispatcher.
		/// </summary>
		IMessageDispatcher Dispatcher { get; set; }

		/// <summary>
		/// Connects to the game server.
		/// </summary>
		/// <param name="address">Server address.</param>
		/// <param name="userName">User name.</param>
		/// <param name="cookie">Authentication cookie.</param>
		/// <exception cref="NetworkException">Thrown when connection attempt is unsuccessful.</exception>
		void Connect(NetworkAddress address, string userName, byte[] cookie);

		/// <summary>
		/// Closes the connection to the server.
		/// </summary>
		void Close();

		/// <summary>
		/// Sends message to the game server.
		/// </summary>
		/// <typeparam name="TMessage">Message type.</typeparam>
		/// <param name="message">Message.</param>
		/// <exception cref="NotSupportedException">Thrown when given message type is not supported by the protocol.</exception>
		void Send<TMessage>(TMessage message);
	}
}

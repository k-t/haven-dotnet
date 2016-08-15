namespace Haven.Messaging
{
	public interface IMessageSource
	{
		void Subscribe(IMessageHandler handler);

		void Unsubscribe(IMessageHandler handler);
	}
}

namespace Haven.Messaging
{
	public interface IMessageHandler
	{
		bool CanHandle(object message);

		void Handle(object message);
	}
}

namespace Haven.Messaging
{
	public interface IMessageDispatcher
	{
		void Dispatch<TMessage>(TMessage message);
	}
}

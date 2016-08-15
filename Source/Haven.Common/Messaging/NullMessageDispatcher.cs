namespace Haven.Messaging
{
	public class NullMessageDispatcher : IMessageDispatcher
	{
		public static readonly NullMessageDispatcher Instance = new NullMessageDispatcher();

		public void Dispatch<TMessage>(TMessage message)
		{
		}
	}
}

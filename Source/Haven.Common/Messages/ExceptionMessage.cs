using System;

namespace Haven.Messages
{
	public class ExceptionMessage
	{
		public ExceptionMessage(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception { get; set; }
	}
}

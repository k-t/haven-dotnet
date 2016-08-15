using System;
using System.Threading;

namespace Haven.Utils
{
	public abstract class BackgroundTask
	{
		private volatile bool isCancelled;
		private readonly Thread thread;

		protected BackgroundTask(string name)
		{
			thread = new Thread(Start);
			thread.IsBackground = true;
			thread.Name = name;
		}

		public event Action Finished;
		public event Action<Exception> Crashed;

		protected bool IsCancelled
		{
			get { return isCancelled; }
		}

		public void Run()
		{
			thread.Start();
		}

		public void Stop()
		{
			// TODO: what if task is a bad one and will not abort afterwards?
			isCancelled = true;
		}

		protected abstract void OnStart();

		private void Start()
		{
			try
			{
				OnStart();
				Finished.Raise();
			}
			catch (Exception e)
			{
				Crashed.Raise(e);
			}
		}
	}
}

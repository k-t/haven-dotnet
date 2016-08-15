using System;

namespace Haven.Utils
{
	public delegate bool Promise<T>(out T value);

	public class Delayed<T>
	{
		private T value;
		private Promise<T> promise;

		public Delayed()
		{
		}

		public Delayed(T value)
		{
			this.value = value;
		}

		public Delayed(Promise<T> promise)
		{
			if (promise == null)
				throw new ArgumentNullException(nameof(promise));
			this.promise = promise;
			this.value = default(T);
		}

		public T Value
		{
			get
			{
				if (promise != null && promise(out value))
					// ensure that closures are not lingering
					promise = null;
				return value;
			}
		}
	}

	public static class DelayedExtensions
	{
		public static Delayed<U> Select<T, U>(this Delayed<T> delayed, Func<T, U> selector)
		{
			return new Delayed<U>((out U value) => {
				if (delayed.Value != null)
				{
					value = selector(delayed.Value);
					return true;
				}
				value = default(U);
				return false;
			});
		}
	}
}

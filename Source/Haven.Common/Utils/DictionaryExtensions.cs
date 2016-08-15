using System.Collections.Generic;

namespace Haven
{
	public static class DictionaryExtensions
	{
		public static bool Remove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, out TValue value)
		{
			if (dict.TryGetValue(key, out value))
			{
				dict.Remove(key);
				return true;
			}
			return false;
		}
}
}

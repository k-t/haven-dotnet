using System.Collections.Generic;

namespace Haven.Resources
{
	public interface IEnumerableResourceSource : IResourceSource
	{
		/// <summary>
		/// Allows to enumerate all resources contained in the source.
		/// </summary>
		IEnumerable<string> EnumerateAll();
	}
}


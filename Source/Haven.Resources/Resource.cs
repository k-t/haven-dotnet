using System.Collections.Generic;
using System.Linq;

namespace Haven.Resources
{
	public class Resource
	{
		private readonly int version;
		private readonly List<object> layers;

		public Resource(int version, IEnumerable<object> layers)
		{
			this.version = version;
			this.layers = new List<object>(layers);
		}

		public int Version
		{
			get { return version; }
		}
		
		public T GetLayer<T>()
		{
			return GetLayers<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetLayers<T>()
		{
			return layers.OfType<T>();
		}

		public IEnumerable<object> GetLayers()
		{
			return layers;
		}
	}
}


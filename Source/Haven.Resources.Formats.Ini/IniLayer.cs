using System.Collections.Generic;

namespace Haven.Resources.Formats.Ini
{
	public class IniLayer
	{
		private readonly object data;
		private readonly IDictionary<string, string> externalFiles;

		public IniLayer(object data, IDictionary<string, string> externalFiles)
		{
			this.data = data;
			this.externalFiles = externalFiles;
		}

		public object Data
		{
			get { return data; }
		}

		public IDictionary<string, string> ExternalFiles
		{
			get { return externalFiles; }
		}
	}
}

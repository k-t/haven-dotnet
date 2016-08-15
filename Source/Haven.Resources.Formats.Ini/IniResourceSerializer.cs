using System;
using System.IO;

namespace Haven.Resources.Formats.Ini
{
	public class IniResourceSerializer : IResourceSerializer
	{
		private readonly IFileSource fileSource;

		public IniResourceSerializer(IFileSource fileSource)
		{
			this.fileSource = fileSource;
		}

		public Resource Deserialize(Stream inputStream)
		{
			var res = new IniResource();
			res.Load(inputStream, fileSource);
			return res.ToResource();
		}

		public void Serialize(Resource res, Stream outputStream)
		{
			// some layers in INI format should contain references to external files
			// so it's not possible to write them into a single stream
			throw new NotSupportedException();
		}
	}
}

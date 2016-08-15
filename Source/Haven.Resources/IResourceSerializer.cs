using System.IO;

namespace Haven.Resources
{
	public interface IResourceSerializer
	{
		Resource Deserialize(Stream inputStream);
		void Serialize(Resource res, Stream outputStream);
	}
}

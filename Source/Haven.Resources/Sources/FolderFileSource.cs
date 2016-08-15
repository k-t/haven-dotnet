using System.IO;

namespace Haven.Resources
{
	public class FolderFileSource : IFileSource
	{
		public FolderFileSource()
		{
		}

		public FolderFileSource(string basePath)
		{
			BasePath = basePath;
		}

		public string BasePath { get; set; }

		public byte[] Read(string path)
		{
			return File.ReadAllBytes(GetFullPath(path));
		}

		public void Write(string path, byte[] bytes)
		{
			using (var fs = File.Open(GetFullPath(path), FileMode.Create))
				fs.Write(bytes, 0, bytes.Length);
		}

		private string GetFullPath(string path)
		{
			return !string.IsNullOrEmpty(BasePath)
				? Path.Combine(BasePath, path)
				: path;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using Haven.Resources.Formats.Binary;

namespace Haven.Resources
{
	public class FolderSource : IEnumerableResourceSource
	{
		private readonly string basePath;
		private readonly FolderFileSource fileSource;
		private readonly Dictionary<string, IResourceSerializer> serializers;

		public FolderSource(string path)
		{
			if (!Directory.Exists(path))
				throw new ArgumentException("Specified path doesn't refer to an existing folder");

			basePath = Path.GetFullPath(path);
			// add directory separator to handle relative paths correctly
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				basePath += Path.DirectorySeparatorChar;
			fileSource = new FolderFileSource();

			serializers = new Dictionary<string, IResourceSerializer>();
			AddSerializer(".res", new BinaryResourceSerializer());
		}

		public string BasePath
		{
			get { return basePath; }
		}

		public string Description
		{
			get { return $"[Folder]{basePath}"; }
		}

		public IFileSource FileSource
		{
			get { return fileSource; }
		}

		public void AddSerializer(string fileExtension, IResourceSerializer serializer)
		{
			serializers[fileExtension] = serializer;
		}

		public IEnumerable<string> EnumerateAll()
		{
			foreach (var fileName in Directory.GetFiles(basePath, "*.*", SearchOption.AllDirectories))
			{
				var ext = Path.GetExtension(fileName);
				if (!string.IsNullOrEmpty(ext) && serializers.ContainsKey(ext))
					yield return ToResourceName(fileName);
			}
		}

		public Resource Get(string resName)
		{
			foreach (var extension in serializers.Keys)
			{
				var fileName = ToFileName(resName, extension);
				fileSource.BasePath = Path.Combine(basePath, Path.GetDirectoryName(resName));
				if (File.Exists(fileName))
				{
					var serializer = serializers[extension];
					using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
						return serializer.Deserialize(fileStream);
				}
			}
			return null;
		}

		public void Dispose()
		{
		}

		private string ToFileName(string resName, string extension)
		{
			return Path.Combine(basePath, resName) + extension;
		}

		private string ToResourceName(string fileName)
		{
			var baseUri = new Uri(basePath);
			var objectName = Path.GetFileNameWithoutExtension(fileName);
			var objectPath = Path.GetDirectoryName(fileName);
			var objectUri = new Uri(Path.Combine(objectPath, objectName));
			return baseUri.MakeRelativeUri(objectUri).ToString();
		}
	}
}


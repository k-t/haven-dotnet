using System.Collections.Generic;
using System.IO;
using Haven.Resources.Formats.Binary;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Haven.Resources
{
	public class JarSource : IEnumerableResourceSource
	{
		private const string NamePrefix = "res/";
		private const string NameSuffix = ".res";

		private readonly ZipFile zip;

		public JarSource(string path)
		{
			zip = new ZipFile(path);
		}

		public string Path
		{
			get { return zip.Name; }
		}

		public string Description
		{
			get { return $"[JAR]{zip.Name}"; }
		}

		public Resource Get(string resourceName)
		{
			var serializer = new BinaryResourceSerializer();
			var entryName = GetEntryName(resourceName);
			var entry = zip.GetEntry(entryName);
			if (entry == null)
				throw new ResourceException($"Entry '{entryName}' not found");

			// read to the buffer whole entry
			var ms = new MemoryStream();
			var buffer = new byte[4096];
			StreamUtils.Copy(zip.GetInputStream(entry), ms, buffer);

			ms.Position = 0;
			return serializer.Deserialize(ms);
		}

		public IEnumerable<string> EnumerateAll()
		{
			foreach (ZipEntry entry in zip)
				if (IsResourceName(entry.Name))
					yield return GetResourceName(entry.Name);
		}

		public void Dispose()
		{
			zip.Close();
		}

		private static string GetEntryName(string resourceName)
		{
			return NamePrefix + resourceName + NameSuffix;
		}

		private static bool IsResourceName(string entryName)
		{
			return entryName.StartsWith(NamePrefix) &&
				entryName.EndsWith(NameSuffix);
		}

		private static string GetResourceName(string entryName)
		{
			int startIndex = NamePrefix.Length;
			int count = entryName.Length - NameSuffix.Length - startIndex;
			return entryName.Substring(startIndex, count);
		}
	}
}


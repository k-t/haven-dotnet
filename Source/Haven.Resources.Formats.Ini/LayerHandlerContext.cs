using System;
using System.Collections.Generic;

namespace Haven.Resources.Formats.Ini
{
	public class LayerHandlerContext
	{
		private readonly IFileSource externalFileSource;
		private readonly IDictionary<string, string> externalFiles;

		public LayerHandlerContext(IFileSource externalFileSource, IDictionary<string, string> externalFiles)
		{
			this.externalFileSource = externalFileSource;
			this.externalFiles = externalFiles;
		}

		public bool HasExternalFile(string fileKey)
		{
			return !string.IsNullOrEmpty(GetFileName(fileKey));
		}

		public byte[] LoadExternalFile(string fileKey)
		{
			var fileName = GetFileName(fileKey);
			return externalFileSource.Read(fileName);
		}

		public void SaveExternalFile(string fileKey, byte[] data)
		{
			var fileName = GetFileName(fileKey);
			externalFileSource.Write(fileName, data);
		}

		private string GetFileName(string fileKey)
		{
			string fileName;
			if (externalFiles.TryGetValue(fileKey, out fileName))
				return fileName;
			throw new ArgumentException($"Invalid external file key '{fileKey}'", nameof(fileKey));
		}
	}
}

using System.Collections.Generic;

namespace Haven.Resources.Formats.Ini
{
	public class IniResourceGenerator
	{
		private readonly LayerHandlerProvider handlers = new LayerHandlerProvider();

		public IniResource Generate(Resource res, string resName)
		{
			var result = new IniResource();
			result.Version = res.Version;

			int i = 0;
			foreach (var data in res.GetLayers())
			{
				var handler = handlers.Get(data);
				if (handler == null)
					continue;

				// generate file names for external files
				var externalFiles = new Dictionary<string, string>();
				foreach (var key in handler.ExternalFileKeys)
				{
					var ext = handler.GetExternalFileExtension(key, data);
					externalFiles[key] = $"{resName}_{i}{ext}";
				}

				result.Layers.Add(new IniLayer(data, externalFiles));
				i++;
			}

			return result;
		}
	}
}

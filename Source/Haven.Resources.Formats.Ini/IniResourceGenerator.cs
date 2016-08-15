using System.IO;

namespace Haven.Resources.Formats.Ini
{
	public class IniResourceGenerator
	{
		private readonly IniLayerHandlerProvider handlers = new IniLayerHandlerProvider();

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

				var layer = handler.Create(data);

				// modify file names to be unique
				foreach (var fileKey in layer.Files.AllKeys)
				{
					var fileName = layer.Files[fileKey];
					var ext = Path.GetExtension(fileName);
					layer.Files[fileKey] = $"{resName}_{i}{ext}";
				}

				result.Layers.Add(layer);
				i++;
			}
			
			return result;
		}
	}
}

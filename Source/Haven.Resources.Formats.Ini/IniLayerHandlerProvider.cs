using System.Collections.Generic;
using Haven.Resources.Formats.Binary;
using Haven.Resources.Formats.Ini.Layers;

namespace Haven.Resources.Formats.Ini
{
	public class IniLayerHandlerProvider
	{
		private readonly IBinaryLayerHandlerProvider binaryHandlerProvider;
		private readonly List<IIniLayerHandler> iniHandlers;

		public IniLayerHandlerProvider() : this(new BinaryLayerHandlerProvider())
		{
		}

		public IniLayerHandlerProvider(IBinaryLayerHandlerProvider binaryHandlerProvider)
		{
			this.binaryHandlerProvider = binaryHandlerProvider;
			iniHandlers = new List<IIniLayerHandler>();
			// built-in ini layers
			Add(new ImageLayerHandler());
			Add(new FontLayerHandler());
			Add(new TexLayerHandler());
			Add(new TileLayerHandler());
			Add(new TilesetLayerHandler());
			Add(new CodeLayerHandler());
			Add(new CodeEntryHandler());
			Add(new TextLayerHandler());
			Add(new TooltipLayerHandler());
			Add(new AudioLayerHandler());
			Add(new ActionLayerHandler());
			Add(new NinepatchLayerHandler());
		}

		public IIniLayerHandler GetByName(string sectionName)
		{
			var handler = iniHandlers.Find(x => x.SectionName == sectionName);
			if (handler != null)
				return handler;
			if (sectionName.StartsWith(BinLayerHandler.Prefix))
			{
				var layerName = sectionName.Remove(0, BinLayerHandler.Prefix.Length);
				var binaryHandler = binaryHandlerProvider.GetByName(layerName);
				return new BinLayerHandler(binaryHandler);
			}
			return null;
		}

		public IIniLayerHandler Get(object data)
		{
			var handler = iniHandlers.Find(x => x.Type == data.GetType());
			if (handler != null)
				return handler;
			var binaryHandler = binaryHandlerProvider.Get(data);
			if (binaryHandler != null)
				return new BinLayerHandler(binaryHandler);
			return null;
		}

		private void Add(IIniLayerHandler handler)
		{
			iniHandlers.Add(handler);
		}
	}
}

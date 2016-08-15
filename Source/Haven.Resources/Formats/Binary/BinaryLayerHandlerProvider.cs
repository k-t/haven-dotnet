using System;
using System.Collections.Generic;
using Haven.Resources.Formats.Binary.Layers;

namespace Haven.Resources.Formats.Binary
{
	/// <summary>
	/// Default implementation of <see cref="IBinaryLayerHandlerProvider"/>.
	/// </summary>
	public class BinaryLayerHandlerProvider : IBinaryLayerHandlerProvider
	{
		protected readonly List<IBinaryLayerHandler> items;

		public BinaryLayerHandlerProvider()
		{
			items = new List<IBinaryLayerHandler>();
			// register built-in handlers
			Add(new ActionLayerHandler());
			Add(new AnimLayerHandler());
			Add(new ImageLayerHandler());
			Add(new NegLayerHandler());
			Add(new TextLayerHandler());
			Add(new TileLayerHandler());
			Add(new TilesetLayerHandler());
			Add(new TooltipLayerHandler());
			Add(new FontLayerHandler());
			Add(new NinepatchLayerHandler());
			Add(new AudioLayerHandler());
			Add(new Audio2LayerHandler()); // supersedes AudioDataHandler
			Add(new CodeLayerHandler());
			Add(new CodeEntryLayerHandler());
			Add(new Tileset2LayerHandler());
			Add(new MidiLayerHandler());
			Add(new FoodEventLayerHandler());
			Add(new MeshLayerHandler());
			Add(new TexLayerHandler());
			Add(new Vertex2LayerHandler());
			Add(new PoseLayerHandler());
			Add(new SkeletonLayerHandler());
			Add(new Material2LayerHandler());
		}

		public void Add(IBinaryLayerHandler handler)
		{
			items.Add(handler);
		}

		public virtual IBinaryLayerHandler GetByName(string layerName)
		{
			return items.FindLast(x => x.LayerName == layerName)
				?? new UnknownLayerHandler(layerName);
		}

		public virtual IBinaryLayerHandler GetByType(Type layerType)
		{
			return items.FindLast(x => x.LayerType == layerType);
		}

		public virtual IBinaryLayerHandler Get(object layer)
		{
			return (layer is UnknownLayer)
				? new UnknownLayerHandler((UnknownLayer)layer)
				: GetByType(layer.GetType());
		}
	}
}

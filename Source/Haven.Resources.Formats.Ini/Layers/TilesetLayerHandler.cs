using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TilesetLayerHandler : GenericLayerHandler<TilesetLayer>
	{
		public TilesetLayerHandler() : base("tileset")
		{
		}

		protected override void Init(IniLayer layer, TilesetLayer data)
		{
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = new TilesetLayer();
			data.HasTransitions = keys.GetBool("has_transitions", false);
			layer.Data = data;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (TilesetLayer)layer.Data;
			keys.Add("has_transitions", data.HasTransitions.ToString());
		}
	}
}

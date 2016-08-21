using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class TilesetLayerHandler : GenericLayerHandler<TilesetLayer>
	{
		public TilesetLayerHandler() : base("tileset")
		{
		}

		protected override TilesetLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new TilesetLayer { HasTransitions = iniData.GetBool("has_transitions", false) };
		}

		protected override void Save(IniKeyCollection iniData, TilesetLayer data, LayerHandlerContext context)
		{
			iniData.Add("has_transitions", data.HasTransitions.ToString());
		}
	}
}

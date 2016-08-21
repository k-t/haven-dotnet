using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class NinepatchLayerHandler : GenericLayerHandler<NinepatchLayer>
	{
		public NinepatchLayerHandler() : base("ninepatch")
		{
		}

		protected override NinepatchLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			return new NinepatchLayer
			{
				Top = iniData.GetByte("top", 0),
				Bottom = iniData.GetByte("bottom", 0),
				Left = iniData.GetByte("left", 0),
				Right = iniData.GetByte("right", 0)
			};
		}

		protected override void Save(IniKeyCollection iniData, NinepatchLayer data, LayerHandlerContext context)
		{
			iniData.Add("top", data.Top);
			iniData.Add("bottom", data.Bottom);
			iniData.Add("left", data.Left);
			iniData.Add("right", data.Right);
		}
	}
}

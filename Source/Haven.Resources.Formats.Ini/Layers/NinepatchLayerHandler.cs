using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	internal class NinepatchLayerHandler : GenericLayerHandler<NinepatchLayer>
	{
		private const string NinepatchSectionName = "ninepatch";

		public NinepatchLayerHandler() : base(NinepatchSectionName)
		{
		}

		protected override void Init(IniLayer layer, NinepatchLayer data)
		{
		}

		protected override void Load(IniLayer layer, IniKeyCollection attrs, IFileSource fileSource)
		{
			var data = new NinepatchLayer();
			data.Top = attrs.GetByte("top", 0);
			data.Bottom = attrs.GetByte("bottom", 0);
			data.Left = attrs.GetByte("left", 0);
			data.Right = attrs.GetByte("right", 0);
			layer.Data = data;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (NinepatchLayer)layer.Data;
			keys.Add("top", data.Top);
			keys.Add("bottom", data.Bottom);
			keys.Add("left", data.Left);
			keys.Add("right", data.Right);
		}
	}
}

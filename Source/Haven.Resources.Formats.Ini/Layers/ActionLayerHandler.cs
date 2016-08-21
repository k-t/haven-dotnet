using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class ActionLayerHandler : GenericLayerHandler<ActionLayer>
	{
		public ActionLayerHandler() : base("action")
		{
		}

		protected override ActionLayer Load(IniKeyCollection iniData, LayerHandlerContext context)
		{
			var data = new ActionLayer();

			data.Name = iniData.GetString("name");
			data.Hotkey = iniData.GetChar("hotkey");
			var parent = iniData.GetString("parent", "");
			if (!string.IsNullOrEmpty(parent))
			{
				var parts = parent.Split(':');
				data.Parent = new ResourceRef(parts[0], ushort.Parse(parts[1]));
			}
			data.Prerequisite = iniData.GetString("prereq", string.Empty);
			data.Verbs = iniData.GetString("verbs", "")?.Split(',');

			return data;
		}

		protected override void Save(IniKeyCollection iniData, ActionLayer data, LayerHandlerContext context)
		{
			iniData.Add("name", data.Name);
			iniData.Add("hotkey", data.Hotkey);
			if (!data.Parent.IsEmpty())
				iniData.Add("parent", $"{data.Parent.Name}:{data.Parent.Version}");
			if (!string.IsNullOrEmpty(data.Prerequisite))
				iniData.Add("prereq", data.Prerequisite);
			if (data.Verbs != null && data.Verbs.Length > 0)
				iniData.Add("verbs", string.Join(",", data.Verbs));
		}
	}
}

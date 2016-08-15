using MadMilkman.Ini;

namespace Haven.Resources.Formats.Ini.Layers
{
	public class ActionLayerHandler : GenericLayerHandler<ActionLayer>
	{
		public ActionLayerHandler() : base("action")
		{
		}

		protected override void Init(IniLayer layer, ActionLayer data)
		{
		}

		protected override void Load(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = new ActionLayer();
			data.Name = keys.GetString("name");
			data.Hotkey = keys.GetChar("hotkey");
			var parent = keys.GetString("parent", "");
			if (!string.IsNullOrEmpty(parent))
			{
				var parts = parent.Split(':');
				data.Parent = new ResourceRef(parts[0], ushort.Parse(parts[1]));
			}
			data.Prerequisite = keys.GetString("prereq", string.Empty);
			data.Verbs = keys.GetString("verbs", "")?.Split(',');
			layer.Data = data;
		}

		protected override void Save(IniLayer layer, IniKeyCollection keys, IFileSource fileSource)
		{
			var data = (ActionLayer)layer.Data;
			keys.Add("name", data.Name);
			keys.Add("hotkey", data.Hotkey);
			if (!data.Parent.IsEmpty())
				keys.Add("parent", $"{data.Parent.Name}:{data.Parent.Version}");
			if (!string.IsNullOrEmpty(data.Prerequisite))
				keys.Add("prereq", data.Prerequisite);
			if (data.Verbs != null && data.Verbs.Length > 0)
				keys.Add("verbs", string.Join(",", data.Verbs));
		}
	}
}

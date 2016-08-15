using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class ActionLayerHandler : GenericLayerHandler<ActionLayer>
	{
		public ActionLayerHandler() : base("action")
		{
		}

		protected override ActionLayer Deserialize(BinaryDataReader reader)
		{
			var action = new ActionLayer();
			action.Parent = new ResourceRef(reader.ReadCString(), reader.ReadUInt16());
			action.Name = reader.ReadCString();
			action.Prerequisite = reader.ReadCString();
			action.Hotkey = (char)reader.ReadUInt16();
			action.Verbs = new string[reader.ReadUInt16()];
			for (int i = 0; i < action.Verbs.Length; i++)
				action.Verbs[i] = reader.ReadCString();
			return action;
		}

		protected override void Serialize(BinaryDataWriter writer, ActionLayer action)
		{
			writer.WriteCString(action.Parent.Name ?? "");
			writer.Write(action.Parent.Version);
			writer.WriteCString(action.Name);
			writer.WriteCString(action.Prerequisite);
			writer.Write((ushort)action.Hotkey);
			writer.Write((ushort)action.Verbs.Length);
			foreach (var verb in action.Verbs)
				writer.WriteCString(verb);
		}
	}
}
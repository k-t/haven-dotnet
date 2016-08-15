using System;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary
{
	public interface IBinaryLayerHandler
	{
		string LayerName { get; }
		Type LayerType { get; }
		object Deserialize(BinaryDataReader reader);
		void Serialize(BinaryDataWriter writer, object layer);
	}
}


using System;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public abstract class GenericLayerHandler<T> : IBinaryLayerHandler
	{
		protected GenericLayerHandler(string layerName)
		{
			LayerName = layerName;
		}

		public string LayerName { get; }

		public Type LayerType
		{
			get { return typeof(T); }
		}

		protected abstract T Deserialize(BinaryDataReader reader);

		protected abstract void Serialize(BinaryDataWriter writer, T layer);

		#region IBinaryLayerHandler
		
		object IBinaryLayerHandler.Deserialize(BinaryDataReader reader)
		{
			return Deserialize(reader);
		}

		void IBinaryLayerHandler.Serialize(BinaryDataWriter writer, object layer)
		{
			Serialize(writer, (T)layer);
		}

		#endregion
	}
}

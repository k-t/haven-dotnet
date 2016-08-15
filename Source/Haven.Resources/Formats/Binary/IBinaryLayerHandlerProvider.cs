using System;

namespace Haven.Resources.Formats.Binary
{
	public interface IBinaryLayerHandlerProvider
	{
		/// <summary>
		/// Returns handler for the resource layer with the specified name.
		/// </summary>
		/// <returns></returns>
		IBinaryLayerHandler GetByName(string layerName);

		/// <summary>
		/// Returns handler for the resource layer of the specified type.
		/// </summary>
		IBinaryLayerHandler GetByType(Type layerType);

		/// <summary>
		/// Returns handler for the specified layer object.
		/// </summary>
		IBinaryLayerHandler Get(object layer);
	}
}

namespace Haven.Resources
{
	/// <summary>
	/// Legacy tileset data.
	/// </summary>
	public class TilesetLayer
	{
		public bool HasTransitions { get; set; }

		public FlavorObjectData[] FlavorObjects { get; set; }

		public ushort FlavorDensity { get; set; }
	}

	public struct FlavorObjectData
	{
		public ResourceRef Resource { get; set; }

		public byte Weight { get; set; }
	}
}

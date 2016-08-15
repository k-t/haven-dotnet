namespace Haven.Resources
{
	public class Tileset2Layer
	{
		public Tileset2Layer()
		{
			TilerName = "gnd";
			TilerAttributes = new object[0];
			Tags = new string[0];
			FlavorObjects = new FlavorObjectData[0];
		}

		public string TilerName { get; set; }
		public object[] TilerAttributes { get; set; }
		public string[] Tags { get; set; }
		public ushort FlavorDensity { get; set; }
		public FlavorObjectData[] FlavorObjects { get; set; }
	}
}

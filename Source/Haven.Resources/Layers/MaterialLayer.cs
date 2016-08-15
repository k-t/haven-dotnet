namespace Haven.Resources
{
	public class MaterialLayer
	{
		public ushort Id { get; set; }
		public bool IsLinear { get; set; }
		public bool IsMipmap { get; set; }
		public Material[] Materials { get; set; }

		public class Material
		{
			public string Name { get; set; }
			public object[] Params { get; set; }
		}
	}
}

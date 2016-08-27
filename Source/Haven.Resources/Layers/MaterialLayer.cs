namespace Haven.Resources
{
	/// <summary>
	/// Material data.
	/// </summary>
	public class MaterialLayer
	{
		public ushort Id { get; set; }

		public bool IsLinear { get; set; }

		public bool IsMipmap { get; set; }

		public Part[] Parts { get; set; }

		public class Part
		{
			public string Name { get; set; }

			public object[] Properties { get; set; }
		}
	}
}

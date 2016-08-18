namespace Haven.Protocols.Hafen.Messages
{
	public class UpdateLighting
	{
		public Color Ambient { get; set; }

		public Color Specular { get; set; }

		public Color Diffuse { get; set; }

		public double Angle { get; set; }

		public double Elevation { get; set; }

		public bool IsIncremental { get; set; }
	}
}

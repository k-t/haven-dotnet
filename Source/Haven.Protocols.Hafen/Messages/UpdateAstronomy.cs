namespace Haven.Protocols.Hafen.Messages
{
	public class UpdateAstronomy
	{
		public double Day { get; set; }

		public double Year { get; set; }

		public double MoonPhase { get; set; }

		public bool IsNight { get; set; }

		public Color MoonColor { get; set; }
	}
}
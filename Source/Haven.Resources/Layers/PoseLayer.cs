namespace Haven.Resources
{
	public class PoseLayer
	{
		public short Id { get; set; }
		public byte Flags { get; set; }
		public byte Mode { get; set; }
		public double Length { get; set; }
		public double Speed { get; set; }
		public PoseEffect[] Effects { get; set; }
		public PoseTrack[] Tracks { get; set; }
	}

	public class PoseEffect
	{
		public PoseEvent[] Events { get; set; }
	}

	public class PoseTrack
	{
		public string BoneName { get; set; }
		public PoseFrame[] Frames { get; set; }
	}

	public class PoseFrame
	{
		public double Time { get; set; }
		public double[] Translation { get; set; }
		public double RotationAngle { get; set; }
		public double[] RotationAxis { get; set; }
	}

	public class PoseEvent
	{
		public double Time { get; set; }
		public byte Type { get; set; }
		public ResourceRef ResRef { get; set; }
		public byte[] Data { get; set; }
		public string Id { get; set; }
	}
}

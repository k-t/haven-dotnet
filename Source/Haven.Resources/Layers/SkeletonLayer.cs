namespace Haven.Resources
{
	public class SkeletonLayer
	{
		public SkeletonBone[] Bones { get; set; }
	}

	public class SkeletonBone
	{
		public string Name { get; set; }
		public float[] Position { get; set; }
		public float[] RotationAxis { get; set; }
		public float RotationAngle { get; set; }
		public string Parent { get; set; }
	}
}

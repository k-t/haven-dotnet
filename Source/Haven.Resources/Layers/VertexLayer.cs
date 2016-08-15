namespace Haven.Resources
{
	public class VertexLayer
	{
		public byte Flags { get; set; } /* ??? */
		public ushort VertexCount { get; set; }
		public float[] Positions { get; set; }
		public float[] Normals { get; set; }
		public float[] Tangents { get; set; }
		public float[] Bitangents { get; set; }
		public float[] TexCoords { get; set; }
		public float[] Colors { get; set; }
		public BoneArray Bones { get; set; }

		public class BoneArray
		{
			public byte Mba { get; set; } /* ? */
			public Bone[] Bones { get; set; }
		}

		public class Bone
		{
			public string Name { get; set; }
			public BoneVertex[] Vertices { get; set; }
		}

		public class BoneVertex
		{
			public ushort Vn { get; set; } /* ? */
			public float[] Weights { get; set; }
		}
	}
}

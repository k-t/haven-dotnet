using System.Collections.Generic;

namespace Haven.Resources
{
	public class MeshLayer
	{
		public MeshLayer()
		{
			Rdat = new Dictionary<string, string>();
		}

		public short Id { get; set; }

		/* ?? */
		public short Ref { get; set; }

		/* ?? */
		public Dictionary<string, string> Rdat { get; set; }

		/// <summary>
		/// ID of the material used by this mesh.
		/// </summary>
		public short MaterialId { get; set; }

		/// <summary>
		/// Mesh vertex indexes.
		/// </summary>
		public short[] Indexes { get; set; }
	}
}

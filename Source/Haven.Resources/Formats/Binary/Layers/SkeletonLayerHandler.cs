using System.Collections.Generic;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class SkeletonLayerHandler : GenericLayerHandler<SkeletonLayer>
	{
		public SkeletonLayerHandler() : base("skel")
		{
		}

		protected override SkeletonLayer Deserialize(BinaryDataReader reader)
		{
			var bones = new List<SkeletonBone>();
			while (reader.HasRemaining)
			{
				var bone = new SkeletonBone();
				bone.Name = reader.ReadCString();
				bone.Position = new float[3];
				for (int i = 0; i < 3; i++)
					bone.Position[i] = (float)reader.ReadFloat40();
				bone.RotationAxis = new float[3];
				for (int i = 0; i < 3; i++)
					bone.RotationAxis[i] = (float)reader.ReadFloat40();
				bone.RotationAngle = (float)reader.ReadFloat40();
				bone.Parent = reader.ReadCString();
				bones.Add(bone);
			}
			return new SkeletonLayer {Bones = bones.ToArray()};
		}

		protected override void Serialize(BinaryDataWriter writer, SkeletonLayer layer)
		{
			foreach (var bone in layer.Bones)
			{
				writer.WriteCString(bone.Name);
				foreach (var n in bone.Position)
					writer.WriteFloat40(n);
				foreach (var n in bone.RotationAxis)
					writer.WriteFloat40(n);
				writer.WriteFloat40(bone.RotationAngle);
				writer.WriteCString(bone.Parent ?? "");
			}
		}
	}
}

using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class AnimLayerHandler : GenericLayerHandler<AnimLayer>
	{
		public AnimLayerHandler() : base("anim")
		{
		}

		protected override AnimLayer Deserialize(BinaryDataReader reader)
		{
			var anim = new AnimLayer();
			anim.Id = reader.ReadInt16();
			anim.Duration = reader.ReadUInt16();
			var frameCount = reader.ReadUInt16();
			anim.Frames = new short[frameCount];
			for (int i = 0; i < frameCount; i++)
				anim.Frames[i] = reader.ReadInt16();
			return anim;
		}

		protected override void Serialize(BinaryDataWriter writer, AnimLayer anim)
		{
			writer.Write(anim.Id);
			writer.Write(anim.Duration);
			writer.Write((ushort)anim.Frames.Length);
			foreach (var frame in anim.Frames)
				writer.Write(frame);
		}
	}
}
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public class MeshLayerHandler : GenericLayerHandler<MeshLayer>
	{
		public MeshLayerHandler() : base("mesh")
		{
		}

		protected override MeshLayer Deserialize(BinaryDataReader reader)
		{
			var flags = reader.ReadByte();
			if ((flags & ~7) != 0)
				throw new ResourceException($"Unsupported flags in fastmesh: {flags}");
			
			var data = new MeshLayer();
			var indexCount = reader.ReadUInt16();
			data.Indexes = new short[indexCount * 3];
			data.MaterialId = reader.ReadInt16();
			data.Id = ((flags & 2) != 0) ? reader.ReadInt16() : (short)-1;
			data.Ref = ((flags & 4) != 0) ? reader.ReadInt16() : (short)-1;
			for (int i = 0; i < data.Indexes.Length; i++)
				data.Indexes[i] = (short)reader.ReadUInt16();

			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, MeshLayer mesh)
		{
			byte flags = 0;
			if (mesh.Id != -1)
				flags |= 2;
			if (mesh.Ref != -1)
				flags |= 4;

			writer.Write(flags);
			writer.Write((ushort)(mesh.Indexes.Length / 3));
			writer.Write(mesh.MaterialId);
			if (mesh.Id != -1)
				writer.Write(mesh.Id);
			if (mesh.Ref != -1)
				writer.Write(mesh.Ref);
			foreach (var index in mesh.Indexes)
				writer.Write(index);
		}
	}
}

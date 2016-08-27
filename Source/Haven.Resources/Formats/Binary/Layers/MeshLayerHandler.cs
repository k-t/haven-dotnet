using System.Collections.Generic;
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
			if ((flags & ~15) != 0)
				throw new ResourceException($"Unsupported flags in fastmesh: {flags}");

			var data = new MeshLayer();
			var indexCount = reader.ReadUInt16();
			data.Indexes = new short[indexCount * 3];
			data.MaterialId = reader.ReadInt16();
			data.Id = ((flags & 2) != 0) ? reader.ReadInt16() : (short)-1;
			data.Ref = ((flags & 4) != 0) ? reader.ReadInt16() : (short)-1;

			data.RData = new Dictionary<string, string>();
			if ((flags & 8) != 0)
			{
				while (true)
				{
					var key = reader.ReadCString();
					if (string.IsNullOrEmpty(key))
						break;
					data.RData[key] = reader.ReadCString();
				}
			}

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
			if (mesh.RData != null && mesh.RData.Count > 0)
				flags |= 8;

			writer.Write(flags);
			writer.Write((ushort)(mesh.Indexes.Length / 3));
			writer.Write(mesh.MaterialId);
			if ((flags & 2) != 0)
				writer.Write(mesh.Id);
			if ((flags & 4) != 0)
				writer.Write(mesh.Ref);
			if ((flags & 8) != 0)
			{
				foreach (var entry in mesh.RData)
				{
					writer.WriteCString(entry.Key);
					writer.WriteCString(entry.Value);
				}
				writer.WriteCString("");
			}
			foreach (var index in mesh.Indexes)
				writer.Write(index);
		}
	}
}

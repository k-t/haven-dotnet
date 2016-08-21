using System;
using System.Collections.Generic;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	public class Vertex2LayerHandler : GenericLayerHandler<VertexLayer>
	{
		public Vertex2LayerHandler() : base("vbuf2")
		{
		}

		protected override VertexLayer Deserialize(BinaryDataReader reader)
		{
			var layer = new VertexLayer();
			layer.Flags = reader.ReadByte();
			layer.VertexCount = reader.ReadUInt16();
			while (reader.HasRemaining)
				ReadArray(layer, reader);
			return layer;
		}

		protected override void Serialize(BinaryDataWriter writer, VertexLayer layer)
		{
			writer.Write(layer.Flags);
			writer.Write(layer.VertexCount);
			WriteArray(writer, layer.Positions, "pos", WriteFloat);
			WriteArray(writer, layer.Normals, "nrm", WriteFloat);
			WriteArray(writer, layer.Tangents, "tan", WriteFloat);
			WriteArray(writer, layer.Bitangents, "bit", WriteFloat);
			WriteArray(writer, layer.Colors, "col", WriteFloat);
			WriteArray(writer, layer.TexCoords, "tex", WriteFloat);
			WriteArray(writer, layer.OverlayTexCoords, "otex", WriteFloat);

			if (layer.Bones != null)
			{
				writer.WriteCString("bones");
				writer.Write(layer.Bones.Mba);
				foreach (var bone in layer.Bones.Bones)
					WriteBone(writer, bone);
				// end of sequence
				writer.Write((ushort)0);
				writer.Write((ushort)0);
			}
		}

		private static void ReadArray(VertexLayer layer, BinaryDataReader reader)
		{
			var vertexCount = layer.VertexCount;
			var type = reader.ReadCString();
			switch (type)
			{
				case "pos":
					layer.Positions = ReadArray(ReadFloat, reader, vertexCount * 3);
					break;
				case "nrm":
					layer.Normals = ReadArray(ReadFloat, reader, vertexCount * 3);
					break;
				case "tan":
					layer.Tangents = ReadArray(ReadFloat, reader, vertexCount * 3);
					break;
				case "bit":
					layer.Bitangents = ReadArray(ReadFloat, reader, vertexCount * 3);
					break;
				case "col":
					layer.Colors = ReadArray(ReadFloat, reader, vertexCount * 4);
					break;
				case "tex":
					layer.TexCoords = ReadArray(ReadFloat, reader, vertexCount * 2);
					break;
				case "otex":
					layer.OverlayTexCoords = ReadArray(ReadFloat, reader, vertexCount * 2);
					break;
				case "bones":
					layer.Bones = new VertexLayer.BoneArray();
					layer.Bones.Mba = reader.ReadByte();
					var bones = new List<VertexLayer.Bone>();
					while (true)
					{
						var bone = ReadBone(reader);
						if (bone == null)
							break;
						bones.Add(bone);
					}
					layer.Bones.Bones = bones.ToArray();
					break;
				default:
					throw new ResourceException("Unsupported vertex attrib array type: " + type);
			}
		}

		private static T[] ReadArray<T>(Func<BinaryDataReader, T> itemReader, BinaryDataReader reader, int itemCount)
		{
			var array = new T[itemCount];
			for (int i = 0; i < array.Length; i++)
				array[i] = itemReader(reader);
			return array;
		}

		private static float ReadFloat(BinaryDataReader reader)
		{
			return reader.ReadSingle();
		}

		private static VertexLayer.Bone ReadBone(BinaryDataReader reader)
		{
			var boneName = reader.ReadCString();
			if (string.IsNullOrEmpty(boneName))
				return null;
			var vertices = new List<VertexLayer.BoneVertex>();
			while (true)
			{
				var run = reader.ReadUInt16();
				var vn = reader.ReadUInt16();
				if (run == 0)
					break;
				var weights = new float[run];
				for (int i = 0; i < run; i++)
					weights[i] = reader.ReadSingle();
				vertices.Add(new VertexLayer.BoneVertex { Vn = vn, Weights = weights });
			}
			return new VertexLayer.Bone {Name = boneName, Vertices = vertices.ToArray()};
		}

		private static void WriteArray<T>(BinaryDataWriter writer, T[] array, string type, Action<BinaryDataWriter, T> itemWriter)
		{
			if (array == null)
				return;
			writer.WriteCString(type);
			for (int i = 0; i < array.Length; i++)
				itemWriter(writer, array[i]);
		}

		private static void WriteFloat(BinaryDataWriter writer, float value)
		{
			writer.Write(value);
		}

		private static void WriteBone(BinaryDataWriter writer, VertexLayer.Bone bone)
		{
			writer.WriteCString(bone.Name);
			foreach (var v in bone.Vertices)
			{
				writer.Write((ushort)v.Weights.Length);
				writer.Write(v.Vn);
				for (int i = 0; i < v.Weights.Length; i++)
					writer.Write(v.Weights[i]);
			}
		}
	}
}

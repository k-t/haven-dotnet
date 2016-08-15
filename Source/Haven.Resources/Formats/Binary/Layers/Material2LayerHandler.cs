using System.Collections.Generic;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class Material2LayerHandler : GenericLayerHandler<MaterialLayer>
	{
		public Material2LayerHandler() : base("mat2")
		{
		}

		protected override MaterialLayer Deserialize(BinaryDataReader reader)
		{
			var layer = new MaterialLayer();
			layer.Id = reader.ReadUInt16();
			// read materials
			var materials = new List<MaterialLayer.Material>();
			while (reader.HasRemaining)
			{
				var mat = new MaterialLayer.Material();
				mat.Name = reader.ReadCString();
				mat.Params = reader.ReadList();
				materials.Add(mat);

				// WTF, loftar?
				switch (mat.Name)
				{
					case "linear":
						layer.IsLinear = true;
						break;
					case "mipmap":
						layer.IsMipmap = true;
						break;
				}
			}
			layer.Materials = materials.ToArray();
			return layer;
		}

		protected override void Serialize(BinaryDataWriter writer, MaterialLayer layer)
		{
			writer.Write(layer.Id);
			foreach (var mat in layer.Materials)
			{
				writer.WriteCString(mat.Name);
				writer.WriteList(mat.Params);
			}
		}
	}
}

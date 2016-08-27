using System;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary.Layers
{
	internal class TexLayerHandler : GenericLayerHandler<TexLayer>
	{
		private const byte ImagePart = 0;
		private const byte MipmapPart = 1;
		private const byte MagFilterPart = 2;
		private const byte MinFilterPart = 3;
		private const byte MaskPart = 4;

		private static readonly TexMipmap[] Mipmaps = {
			TexMipmap.Average,
			TexMipmap.Average,
			TexMipmap.Random,
			TexMipmap.Cnt,
			TexMipmap.Dav,
		};

		private static readonly TexMagFilter[] MagFilters = {
			TexMagFilter.Nearest,
			TexMagFilter.Linear,
		};

		private static readonly TexMinFilter[] MinFilters = {
			TexMinFilter.Nearest,
			TexMinFilter.Linear,
			TexMinFilter.NearestMipmapNearest,
			TexMinFilter.NearestMipmapLinear,
			TexMinFilter.LinearMipmapNearest,
			TexMinFilter.LinearMipmapLinear,
		};

		public TexLayerHandler() : base("tex")
		{
		}

		protected override TexLayer Deserialize(BinaryDataReader reader)
		{
			var data = new TexLayer();
			data.Id = reader.ReadInt16();
			data.Offset = new Point2D(reader.ReadUInt16(), reader.ReadUInt16());
			data.Size = new Point2D(reader.ReadUInt16(), reader.ReadUInt16());
			data.Mipmap = TexMipmap.None;
			TexMinFilter? minFilter = null;
			TexMagFilter? magFilter = null;
			while (reader.HasRemaining)
			{
				int part = reader.ReadByte();
				switch (part)
				{
					case ImagePart:
						data.ImageData = reader.ReadBytes(reader.ReadInt32());
						break;
					case MipmapPart:
						data.Mipmap = Mipmaps[reader.ReadByte()];
						break;
					case MagFilterPart:
						magFilter = MagFilters[reader.ReadByte()];
						break;
					case MinFilterPart:
						minFilter = MinFilters[reader.ReadByte()];
						break;
					case MaskPart:
						data.MaskImageData = reader.ReadBytes(reader.ReadInt32());
						break;
					default:
						throw new ResourceException($"Unknown texture data part: {part}");
				}

			}
			data.MagFilter = magFilter ?? TexMagFilter.Linear;
			data.MinFilter = minFilter ?? (
				data.Mipmap == TexMipmap.None
				? TexMinFilter.Linear
				: TexMinFilter.LinearMipmapLinear);
			return data;
		}

		protected override void Serialize(BinaryDataWriter writer, TexLayer data)
		{
			writer.Write(data.Id);
			writer.Write((ushort)data.Offset.X);
			writer.Write((ushort)data.Offset.Y);
			writer.Write((ushort)data.Size.X);
			writer.Write((ushort)data.Size.Y);
			// image
			if (data.ImageData != null)
			{
				writer.Write(ImagePart);
				writer.Write(data.ImageData.Length);
				writer.Write(data.ImageData);
			}
			// mipmap
			if (data.Mipmap != TexMipmap.None)
			{
				writer.Write(MipmapPart);
				writer.Write((byte)Array.IndexOf(Mipmaps, data.Mipmap));
			}
			// magfilter
			writer.Write(MagFilterPart);
			writer.Write((byte)Array.IndexOf(MagFilters, data.MagFilter));
			// minfilter
			writer.Write(MinFilterPart);
			writer.Write((byte)Array.IndexOf(MinFilters, data.MinFilter));
			// mask
			if (data.MaskImageData != null)
			{
				writer.Write(MaskPart);
				writer.Write(data.MaskImageData.Length);
				writer.Write(data.MaskImageData);
			}
		}
	}
}

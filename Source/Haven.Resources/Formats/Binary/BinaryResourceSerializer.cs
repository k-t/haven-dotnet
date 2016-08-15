using System.Collections.Generic;
using System.IO;
using Haven.Utils;

namespace Haven.Resources.Formats.Binary
{
	public class BinaryResourceSerializer : IResourceSerializer
	{
		private const string Signature = "Haven Resource 1";

		private readonly IBinaryLayerHandlerProvider handlerProvider;

		public BinaryResourceSerializer() : this(new BinaryLayerHandlerProvider())
		{
		}

		public BinaryResourceSerializer(IBinaryLayerHandlerProvider handlerProvider)
		{
			this.handlerProvider = handlerProvider;
		}

		public Resource Deserialize(Stream inputStream)
		{
			var buffer = new BinaryDataReader(inputStream);

			var sig = new string(buffer.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceException("Invalid signature");

			var version = buffer.ReadUInt16();
			var layers = new List<object>();
			while (true)
			{
				var layer = ReadLayer(buffer);
				if (layer == null)
					break;
				layers.Add(layer);
			}
			return new Resource(version, layers);
		}

		public void Serialize(Resource res, Stream outputStream)
		{
			var buffer = new BinaryDataWriter(outputStream);
			buffer.Write(Signature.ToCharArray());
			buffer.Write((ushort)res.Version);
			foreach (var layer in res.GetLayers())
			{
				var serializer = handlerProvider.Get(layer);
				if (serializer == null)
					throw new ResourceException("Unsupported layer type " + layer.GetType().FullName);
				buffer.WriteCString(serializer.LayerName);
				// write each layer to the temporary buffer
				// to be able to prefix layer length
				using (var ms = new MemoryStream())
				using (var layerBuffer = new BinaryDataWriter(ms))
				{
					serializer.Serialize(layerBuffer, layer);
					buffer.Write((int)ms.Length);
					buffer.Write(ms.ToArray());
				}
			}
		}

		private object ReadLayer(BinaryDataReader buffer)
		{
			var layerName = buffer.ReadCString();
			if (string.IsNullOrEmpty(layerName))
				return null;
			var size = buffer.ReadInt32();
			var layerBuffer = new BinaryDataReader(buffer, size);
			return handlerProvider.GetByName(layerName).Deserialize(layerBuffer);
		}
	}
}

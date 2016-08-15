using System.IO;
using Haven.Utils;
using NUnit.Framework;

namespace Haven.Resources.Formats.Binary
{
	public static class BinarySerializerHelper
	{
		public static object Reserialize(this IBinaryLayerHandler handler, object input)
		{
			object output;
			using (var ms = new MemoryStream())
			{
				handler.Serialize(new BinaryDataWriter(ms), input);
				ms.Position = 0;
				output = handler.Deserialize(new BinaryDataReader(ms));
				Assert.That(ms.Position, Is.EqualTo(ms.Length), "Stream should be read till the end");
			}
			return output;
		}
	}
}

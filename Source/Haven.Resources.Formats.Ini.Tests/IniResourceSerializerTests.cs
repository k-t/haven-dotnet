using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Haven.Resources.Formats.Ini
{
	[TestFixture]
	public class IniResourceSerializerTests
	{
		private const string BasePath = "Samples";

		private IniResourceSerializer serializer;

		[SetUp]
		public void Setup()
		{
			var folderSource = new FolderFileSource();
			folderSource.BasePath = BasePath;
			serializer = new IniResourceSerializer(folderSource);
		}

		[Test]
		public void FontDataTest()
		{
			using (var fs = File.OpenRead(Path.Combine(BasePath, "font.ini")))
			{
				var res = serializer.Deserialize(fs);
				var layers = res.GetLayers().ToArray();

				Assert.That(res.Version, Is.EqualTo(42), "Wrong resource version");
				Assert.That(layers.Length, Is.EqualTo(1), "Wrong number of layers");
				Assert.That(layers[0], Is.InstanceOf(typeof(FontLayer)), "Wrong layer type");

				var fontLayer = (FontLayer)layers[0];
				Assert.That(fontLayer.Bytes.Length, Is.EqualTo(3), "Invalid font data");
			}
		}

		[Test]
		public void ImageDataTest()
		{
			using (var fs = File.OpenRead(Path.Combine(BasePath, "image.ini")))
			{
				var res = serializer.Deserialize(fs);
				var layers = res.GetLayers().ToArray();

				Assert.That(res.Version, Is.EqualTo(2), "Wrong resource version");
				Assert.That(layers.Length, Is.EqualTo(1), "Wrong number of layers");
				Assert.That(layers[0], Is.InstanceOf(typeof(ImageLayer)), "Wrong layer type");

				var imageLayer = (ImageLayer)layers[0];

				Assert.That(imageLayer.Id, Is.EqualTo(-1));
				Assert.That(imageLayer.Z, Is.EqualTo(1));
				Assert.That(imageLayer.SubZ, Is.EqualTo(2));
				Assert.That(imageLayer.Offset, Is.EqualTo(new Point2D(13, 24)));
				using (var bitmap = Image.FromStream(new MemoryStream(imageLayer.Data)))
				{
					Assert.That(bitmap.Width, Is.EqualTo(2), "Wrong image data");
					Assert.That(bitmap.Height, Is.EqualTo(2), "Wrong image data");
				}
			}
		}
	}
}

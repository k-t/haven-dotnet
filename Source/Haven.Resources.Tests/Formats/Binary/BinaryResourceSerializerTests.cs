using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Haven.Resources.Formats.Binary
{
	[TestFixture]
	public class BinaryResourceSerializerTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new Resource(42, new object[] {
				new ActionLayer {
					Hotkey = 'h',
					Name = "Name",
					Parent = new ResourceRef("Parent", 42),
					Prerequisite = "Prerequisite",
					Verbs = new[] { "verb1", "verb2" }
				},
				new TextLayer { Text = "Text1" },
				new ImageLayer {
					Id = 42,
					Offset = new Point2D(1, 2),
					Z = -10,
					SubZ = -20,
					Data = new byte[] { 1, 2, 3, 4, 5 },
					IsLayered = true
				},
				new TextLayer { Text = "Text2" },
				new UnknownLayer("unknown", new byte[] { 3, 4, 21 }), 
			});

			Resource output;
			using (var ms = new MemoryStream())
			{
				var serializer = new BinaryResourceSerializer();
				serializer.Serialize(input, ms);
				ms.Position = 0;
				output = serializer.Deserialize(ms);
			}

			Assert.That(output.Version, Is.EqualTo(input.Version));
			Assert.That(output.GetLayers().Count(), Is.EqualTo(input.GetLayers().Count()));
			Assert.That(output.GetLayer<ActionLayer>(), Is.Not.Null);
			Assert.That(output.GetLayer<ImageLayer>(), Is.Not.Null);
			Assert.That(output.GetLayers<TextLayer>().Count(), Is.EqualTo(2));

			Assert.That(output.GetLayers<UnknownLayer>().Count(), Is.EqualTo(1));
			Assert.That(output.GetLayers<UnknownLayer>().First().LayerName, Is.EqualTo("unknown"));
			Assert.That(output.GetLayers<UnknownLayer>().First().Bytes, Is.EquivalentTo(new byte[] { 3, 4, 21 }));
		}
	}
}

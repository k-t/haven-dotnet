using System.Linq;
using NUnit.Framework;

namespace Haven.Resources.Formats.Ini
{
	[TestFixture]
	public class IniResourceGeneratorTests
	{
		[Test]
		public void GenerateWorks()
		{
			var res = new Resource(3, new object[] {
				new FontLayer { Bytes = new byte[] {1, 2}, Type = 0 },
				new AnimLayer { Duration = 1, Id = 2, Frames = new short[] {3, 4, 5 }},
				new UnknownLayer("unknown", new byte[] { 71, 1 }), 
			});

			var generator = new IniResourceGenerator();
			var iniRes = generator.Generate(res, "foo");

			Assert.That(iniRes.Version, Is.EqualTo(res.Version));
			Assert.That(iniRes.Layers.Count, Is.EqualTo(res.GetLayers().Count()));
		}
	}
}

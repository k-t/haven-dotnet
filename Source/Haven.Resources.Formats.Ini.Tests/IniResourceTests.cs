using System.Linq;
using NUnit.Framework;

namespace Haven.Resources.Formats.Ini
{
	[TestFixture]
	public class IniResourceTests
	{
		[Test]
		public void FromResourceWorks()
		{
			var res = new Resource(3, new object[] {
				new FontLayer { Bytes = new byte[] {1, 2}, Type = 0 },
				new AnimLayer { Duration = 1, Id = 2, Frames = new short[] {3, 4, 5 }},
				new UnknownLayer("unknown", new byte[] { 71, 1 }),
			});

			var iniRes = IniResource.FromResource(res, "foo");

			Assert.That(iniRes.Version, Is.EqualTo(res.Version));
			Assert.That(iniRes.Layers.Count, Is.EqualTo(res.GetLayers().Count()));

			var fontLayer = iniRes.Layers.FirstOrDefault(x => x.Data is FontLayer);
			Assert.That(fontLayer, Is.Not.Null);
			Assert.That(fontLayer.ExternalFiles.Count, Is.EqualTo(1));

			var animLayer = iniRes.Layers.FirstOrDefault(x => x.Data is AnimLayer);
			Assert.That(animLayer, Is.Not.Null);

			var unknownLayer = iniRes.Layers.FirstOrDefault(x => x.Data is UnknownLayer);
			Assert.That(unknownLayer, Is.Not.Null);
		}
	}
}

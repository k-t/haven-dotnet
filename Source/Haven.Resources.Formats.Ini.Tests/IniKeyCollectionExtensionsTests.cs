using MadMilkman.Ini;
using NUnit.Framework;

namespace Haven.Resources.Formats.Ini
{
	[TestFixture]
	public class IniKeyCollectionExtensionsTests
	{
		private IniSection section;

		[SetUp]
		public void Setup()
		{
			var file = new IniFile();
			section = file.Sections.Add("test");
		}

		[Test]
		public void AddInt32FormatIsCorrect()
		{
			section.Keys.Add("int32", 1245456);
			Assert.That(section.Keys["int32"].Value, Is.EqualTo("1245456"));
		}

		[Test]
		public void AddPointFormatIsCorrect()
		{
			section.Keys.Add("point", new Point2D(23, -321));
			Assert.That(section.Keys["point"].Value, Is.EqualTo("23,-321"));
		}
	}
}

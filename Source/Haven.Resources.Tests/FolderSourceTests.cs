using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Haven.Resources
{
	[TestFixture]
	public class FolderSourceTests
	{
		private const string SamplesPath = "Samples/FolderSource";

		[Test]
		public void ThrowsExceptionWhenFolderNotExists(
			[Values("./NotExistingFolder", SamplesPath + "/logo.res")]
			string path)
		{
			Assert.That(() => new FolderSource(path), Throws.ArgumentException);
		}

		[Test]
		public void GetWorks(
			[ValueSource(nameof(GetTestItems))]
			TestItem testItem)
		{
			var source = new FolderSource(SamplesPath);
			var resource = source.Get(testItem.ResourceName);

			Assert.IsNotNull(resource);
			Assert.AreEqual(testItem.ExpectedVersion, resource.Version);
		}

		[Test]
		public void EnumerateAllWorks()
		{
			var source = new FolderSource(SamplesPath);

			var result = source.EnumerateAll().ToList();

			Assert.AreEqual(3, result.Count);
			Assert.IsTrue(result.Contains("pagina"));
			Assert.IsTrue(result.Contains("tooltip"));
			Assert.IsTrue(result.Contains("SubFolder/radar"));
		}

		private IEnumerable<TestItem> GetTestItems
		{
			get
			{
				return new[] {
					new TestItem { ResourceName = "pagina", ExpectedVersion = 1 },
					new TestItem { ResourceName = "SubFolder/radar", ExpectedVersion = 1 }
				};
			}
		}

		public class TestItem
		{
			public string ResourceName { get; set; }
			public int ExpectedVersion { get; set; }
		}
	}
}


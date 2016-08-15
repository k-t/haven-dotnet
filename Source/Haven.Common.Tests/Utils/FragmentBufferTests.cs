using NUnit.Framework;

namespace Haven.Utils
{
	[TestFixture]
	public class FragmentBufferTests
	{
		[Test]
		public void ItWorks()
		{
			var buffer = new FragmentBuffer(20);
			Assert.That(buffer.IsComplete, Is.False, "Buffer is completed without any data");

			buffer.Add(5, new byte[] { 5, 5, 5, 5, 5 }, 0, 5);
			Assert.That(buffer.IsComplete, Is.False);

			buffer.Add(1, new byte[] { 4, 4, 4, 4 }, 0, 4);
			Assert.That(buffer.IsComplete, Is.False);

			buffer.Add(14, new byte[] { 6, 6, 6, 6, 6, 6 }, 0, 6);
			Assert.That(buffer.IsComplete, Is.False);

			buffer.Add(4, new byte[] { 4, 5, 5, 5, 5, 5, 1, 2, 3, 4 }, 0, 10);
			Assert.That(buffer.IsComplete, Is.False);

			buffer.Add(0, new byte[] { 255 }, 0, 1);
			Assert.That(buffer.IsComplete, Is.True);
			Assert.That(buffer.Content, Is.EquivalentTo(new byte[] {
				255, 4, 4, 4, 4, 5, 5, 5, 5, 5, 1, 2, 3, 4, 6, 6, 6, 6, 6, 6
			}));
		}

		[Test]
		public void CompletesWhenFullFragmentAdded()
		{
			var buffer = new FragmentBuffer(5);
			Assert.That(buffer.IsComplete, Is.False, "Buffer is completed without any data");

			buffer.Add(2, new byte[] { 5 }, 0, 1);
			buffer.Add(4, new byte[] { 5 }, 0, 1);

			buffer.Add(0, new byte[] { 5, 5, 5, 5, 5 }, 0, 5);
			Assert.That(buffer.IsComplete, Is.True);
			Assert.That(buffer.Content, Is.EquivalentTo(new byte[] { 5, 5, 5, 5, 5 }));
		}

		[Test]
		public void CompletesWhenFullFragmentAddedFirst()
		{
			var buffer = new FragmentBuffer(5);
			Assert.That(buffer.IsComplete, Is.False, "Buffer is completed without any data");

			buffer.Add(0, new byte[] { 5, 5, 5, 5, 5 }, 0, 5);
			Assert.That(buffer.IsComplete, Is.True);
			Assert.That(buffer.Content, Is.EquivalentTo(new byte[] { 5, 5, 5, 5, 5 }));
		}
	}
}

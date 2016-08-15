using NUnit.Framework;

namespace Haven.Net
{
	[TestFixture]
	public class BinaryMessageTests
	{
		[Test]
		public void ReadWriteTest()
		{
			var input = BinaryMessage.Make(1)
				.String("Test")
				.UInt16(64)
				.String("Test2")
				.Complete();

			var output = new BinaryMessage(1, input.GetData());

			Assert.AreEqual(output.Type, input.Type);

			using (var reader = output.GetReader())
			{
				Assert.AreEqual("Test", reader.ReadCString());
				Assert.AreEqual(64, reader.ReadUInt16());
				Assert.AreEqual("Test2", reader.ReadCString());
			}
		}
	}
}


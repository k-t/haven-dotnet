using NUnit.Framework;

namespace Haven.Utils
{
	[TestFixture]
	public class Float40Tests
	{
		[TestCase(0.0086249999985739123)]
		[TestCase(3.14)]
		public void PositiveConversionIsCorrect(double value)
		{
			var f = new Float40(value);
			Assert.That((double)f, Is.EqualTo(value));
		}

		[TestCase(0.0086249999985739123)]
		[TestCase(3.14)]
		public void NegativeConversionIsCorrect(double value)
		{
			var f = new Float40(-value);
			Assert.That((double)f, Is.EqualTo(-value));
		}
	}
}

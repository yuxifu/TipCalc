using NUnit.Framework;
using TipCalc.Core.Services;

namespace TipCalc.Tests
{
	[TestFixture]
	public class TipServiceTests
    {
        [Test]
        public void TestThatZeroGenerosityReturnsZeroTip()
        {
            //Arrange
            var tip = new Calculation();

            //Act
            var result = tip.TipAmount(42.35, 0);

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestCase(10.25, 10)]
		[TestCase(10.25, 15)]
		[TestCase(10.25, 20)]
		public void TestThatTenGenerosityReturnsTenTip(double subTotal, int generosity )
		{
			//Arrange
			var tip = new Calculation();

			//Act
			var result = tip.TipAmount(subTotal, generosity);

            //Assert
            Assert.AreEqual(subTotal * generosity / 100.0, result);
		}

    }
}

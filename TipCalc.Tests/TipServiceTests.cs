﻿using NUnit.Framework;
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

		[Test]
		public void TestThatTenGenerosityReturnsTenTip()
		{
			//Arrange
			var tip = new Calculation();

			//Act
			var result = tip.TipAmount(42.35, 10);

			//Assert
			Assert.AreEqual(4.235, result);
		}

    }
}

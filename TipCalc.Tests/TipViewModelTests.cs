using System;
using System.Collections.Generic;
using NUnit.Framework;
using MvvmCross.Core.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using Moq;
using TipCalc.Core.ViewModels;
using TipCalc.Core.Services;

namespace TipCalc.Tests
{
    public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
	{
		public readonly List<MvxViewModelRequest> Requests = new List<MvxViewModelRequest>();
		public readonly List<MvxPresentationHint> Hints = new List<MvxPresentationHint>();

		public bool RequestMainThreadAction(Action action)
		{
			action();
			return true;
		}

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            Requests.Add((request));
            return true;
        }

		public bool ChangePresentation(MvxPresentationHint hint)
		{
			throw new NotImplementedException();
		}
	}

	[TestFixture]
    public class TipViewModelTests: MvxIoCSupportingTest
    {
        [Test]
        public void TestThatWhenSubTotalChangesTipIsRecalculated()
        {
            //Arrange
            base.ClearAll();
            var mockTipService = new Mock<ICalculation>();
            mockTipService.Setup(t => t.TipAmount(It.IsAny<double>(), It.IsAny<int>()))
                          .Returns(42.0);
            var tipViewModel = new TipViewModel(mockTipService.Object);

            //Act
            tipViewModel.SubTotal = 12;

            //Assert
            Assert.AreEqual(42.0, tipViewModel.Tip);
        }

		[Test]
		public void TestThatWhenGenerosityChangesTipIsRecalculated()
		{
			//Arrange
			base.ClearAll();
			var mockTipService = new Mock<ICalculation>();
			mockTipService.Setup(t => t.TipAmount(It.IsAny<double>(), It.IsAny<int>()))
						  .Returns(37.0);
			var tipViewModel = new TipViewModel(mockTipService.Object);

			//Act
            tipViewModel.Generosity = 12;

			//Assert
			Assert.AreEqual(37.0, tipViewModel.Tip);
		}

		[Test]
		public void TestThatWhenTipIsRecalculatedThenTipNotificationIsSent()
		{
			//Arrange
			base.ClearAll();
			var mockTipService = new Mock<ICalculation>();
			mockTipService.Setup(t => t.TipAmount(It.IsAny<double>(), It.IsAny<int>()))
						  .Returns(37.0);

            var mockDispatcher = new MockDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(mockDispatcher);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(mockDispatcher);

			var tipViewModel = new TipViewModel(mockTipService.Object);

            var tipChangeCount = 0;
			var generosityChangeCount = 0;
			var subTotalCount = 0;
            tipViewModel.PropertyChanged += (sender, e) =>
            {
                switch(e.PropertyName)
                {
                    case "Tip":
                        tipChangeCount++;
                        break;
					case "SubTotal":
                        subTotalCount++;
						break;
					case "Generosity":
                        generosityChangeCount++;
						break;
				}
            };

			//Act
			tipViewModel.Generosity = 12;

			//Assert
            Assert.AreEqual(1, tipChangeCount);
            Assert.AreEqual(0, subTotalCount);
            Assert.AreEqual(1, generosityChangeCount);
		}


		[Test]
		public void TestThatPayCommandShowsPayViewModelWithCorrectTotal()
		{
			//Arrange
			base.ClearAll();
			var mockTipService = new Mock<ICalculation>();
			mockTipService.Setup(t => t.TipAmount(It.IsAny<double>(), It.IsAny<int>()))
						  .Returns(19);

			var mockDispatcher = new MockDispatcher();
			Ioc.RegisterSingleton<IMvxViewDispatcher>(mockDispatcher);
			Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(mockDispatcher);

            var tipViewModel = new TipViewModel(mockTipService.Object)
            {
                Generosity = 12,
                SubTotal = 10
            };

            //act
            tipViewModel.PayComamnd.Execute(null);

			//Assert
            Assert.AreEqual(1, mockDispatcher.Requests.Count);
            var request = mockDispatcher.Requests[0];
            Assert.AreEqual(typeof(PayViewModel),request.ViewModelType);
            Assert.AreEqual("29",request.ParameterValues["total"]);
		}

	
    }
}

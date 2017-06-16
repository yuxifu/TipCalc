using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using TipCalc.Core.Services;

namespace TipCalc.Core.ViewModels
{
	public class TipViewModel : MvxViewModel
	{
		readonly ICalculation _calculation;

		public TipViewModel(ICalculation calculation)
		{
			_calculation = calculation;
		}

		public override void Start()
		{
			_subTotal = 100;
			_generosity = 10;
			Recalculate();
			base.Start();
		}

        public ICommand PayComamnd
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<PayViewModel>(new Dictionary<string, string>()
                {
                    {"total", (Tip + SubTotal).ToString()}
                }));
            }
        }

		double _subTotal;

		public double SubTotal
		{
			get
			{
				return _subTotal;
			}
			set
			{
				_subTotal = value;
				RaisePropertyChanged(() => SubTotal);
				Recalculate();
			}
		}

		int _generosity;

		public int Generosity
		{
			get
			{
				return _generosity;
			}
			set
			{
				_generosity = value;
				RaisePropertyChanged(() => Generosity);
				Recalculate();
			}
		}

		double _tip;

		public double Tip
		{
			get
			{
				return _tip;
			}
			set
			{
				_tip = value;
				RaisePropertyChanged(() => Tip);
			}
		}

		void Recalculate()
		{
			Tip = _calculation.TipAmount(SubTotal, Generosity);
		}
	}

}

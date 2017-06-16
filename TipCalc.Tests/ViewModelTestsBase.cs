using MvvmCross.Core.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;

public class ViewModelTestsBase : MvxIoCSupportingTest
{
    protected MockDispatcher MockDispatcher
    {
        get;
        private set;
    }

    protected override void AdditionalSetup()
    {
        base.AdditionalSetup();

        // register mock dispatcher
        MockDispatcher = new MockDispatcher();
        Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
        Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);

        // for navigation parsing
        Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
    }
}
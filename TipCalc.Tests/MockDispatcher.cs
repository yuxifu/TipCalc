using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;

// When creating ViewModel or Service test objects, one common requirement is 
// to provide a mock object which implements both IMvxViewDispatcher and 
// IMvxMainThreadDispatcher.These interfaces are required for MvvmCross UI 
// thread marshalling and for MvvmCross ViewModel navigation.This object can 
// be implemented using a class like MockDispatcher:
// src: https://www.mvvmcross.com/documentation/testing/testing?scroll=1499                                                                                                                                                                          
public class MockDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
{
    public readonly List<MvxViewModelRequest> Requests =
        new List<MvxViewModelRequest>();
    public readonly List<MvxPresentationHint> Hints =
        new List<MvxPresentationHint>();

    public bool RequestMainThreadAction(Action action)
    {
        action();
        return true;
    }

    public bool ShowViewModel(MvxViewModelRequest request)
    {
        Requests.Add(request);
        return true;
    }

    public bool ChangePresentation(MvxPresentationHint hint)
    {
        Hints.Add(hint);
        return true;
    }
}

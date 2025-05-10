using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DocuTrack.UI.ViewModels;


namespace DocuTrack.Views

{

    public class BaseViewModel : ObservableObject, IViewModel
    {
       
        private bool _isResizing;
    public bool IsResizing
    {
        get => _isResizing;
        set => SetProperty(ref _isResizing, value);
    }

    }

}
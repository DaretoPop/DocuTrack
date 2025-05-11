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

     private double _scalingFactor = 1.0;
    public double ScalingFactor
        {
            get => _scalingFactor;
            set => SetProperty(ref _scalingFactor, value);
        }

        protected void BindScaling(MainViewModel mainViewModel)
        {
            // Set initial value
            ScalingFactor = mainViewModel.ScalingFactor;

            // Subscribe to scaling changes
            mainViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainViewModel.ScalingFactor))
                {
                    ScalingFactor = mainViewModel.ScalingFactor;
                }
            };
        }

    }

}
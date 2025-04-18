using Avalonia.Rendering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Pages;
using DocuTrack.Views;


namespace DocuTrack.ViewModels {

    public partial class UpsertSailorViewModel : BaseViewModel

    {

            public UpsertSailorViewModel(MainViewModel mainViewModel)
            {
                MainViewModel = mainViewModel;
            }


    [ObservableProperty] private MainViewModel _mainViewModel;


    [RelayCommand]
    private void GoBack()
    {
        MainViewModel.GoToPomorci();
    }


    }
}
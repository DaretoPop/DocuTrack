using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;


public partial class PocetnaViewModel : BaseViewModel

{

public PocetnaViewModel(MainViewModel mainViewModel)
{
    _mainViewModel = mainViewModel;
}


   [ObservableProperty]
   private  MainViewModel? _mainViewModel;


[RelayCommand]
public void GoToProfilPanel()
{
    if(MainViewModel != null)
        {
            MainViewModel.GoToPomorciPanel();

                if(MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = true;
                }
    }
}





};
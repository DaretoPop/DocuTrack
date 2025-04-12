using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;





public partial class PomorciViewModel : BaseViewModel

{


public PomorciViewModel(){}
public PomorciViewModel(MainViewModel mainViewModel)
{
        _mainViewModel = mainViewModel;
}



[ObservableProperty] private  MainViewModel? _mainViewModel;
[ObservableProperty] private bool _isPanelVisible;






[RelayCommand] 
private void GoToKreirajPomorcaPage()
{
        if(MainViewModel != null)
        {
                MainViewModel.CurrentPage = MainViewModel.KreirajKorisnikaPage;
        }
}

[RelayCommand]
private void TogglePanel()
{
    IsPanelVisible = !IsPanelVisible;
}

};
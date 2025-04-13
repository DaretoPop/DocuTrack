using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;


public partial class PrikazProfilaPomorcaPanel : BaseViewModel

{


public PrikazProfilaPomorcaPanel(){}
public PrikazProfilaPomorcaPanel(MainViewModel mainViewModel)
{
        _mainViewModel = mainViewModel;
}


[ObservableProperty] private  MainViewModel? _mainViewModel;



// Back Button to navigate to ProfilPage or PocentaPage(sertifikati)
[RelayCommand] 
public void GoBack()
{
    if(MainViewModel != null)
    {
        if(MainViewModel?.IsPocetnaView == true)
        {
                MainViewModel.CurrentPage = MainViewModel.PocetnaPage;
        }else
        {
                MainViewModel?.GoToPomorci();
        }
    }
}




};
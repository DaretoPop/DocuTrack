using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels;





public partial class DokumentiViewModel : BaseViewModel

{


        [ObservableProperty] 
private bool _isRightPanelVisible;

[RelayCommand]
private void ToggleRightPanel()
{
    IsRightPanelVisible = !IsRightPanelVisible;
}


};
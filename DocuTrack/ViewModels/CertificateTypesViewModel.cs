using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class CertificateTypesViewModel : BaseViewModel

    {

        private CertificateTypesViewModel(){}
        public CertificateTypesViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }


        [ObservableProperty] private MainViewModel _mainViewModel;
        [ObservableProperty] private bool _isRightPanelVisible;

        
        [RelayCommand]
        private void ToggleRightPanel()
        {
            IsRightPanelVisible = !IsRightPanelVisible;
        }


        [RelayCommand]
        private void GoToAddNewDocument()
        {
            MainViewModel.CurrentPage = MainViewModel.AddNewDocumentPage;
        }


    }
}
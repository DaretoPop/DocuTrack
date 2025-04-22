using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class AddNewDocumentViewModel : BaseViewModel

    {

        public AddNewDocumentViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }



        [ObservableProperty] private MainViewModel _mainViewModel;
        [ObservableProperty] private string naziv = string.Empty;


        [RelayCommand] 
        private void GoToBack()
        {
            if(MainViewModel != null)
            {
                MainViewModel.CurrentPage = MainViewModel.DokumentaPage;
            }
        }


    }
}
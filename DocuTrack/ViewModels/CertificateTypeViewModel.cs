using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class CertificateTypeViewModel : BaseViewModel

    {

        public CertificateTypeViewModel(MainViewModel mainViewModel, CertificateType type)
        {
            MainViewModel = mainViewModel;
            _certificateType = type;
        }


        [ObservableProperty] private CertificateType? _certificateType;
        [ObservableProperty] private MainViewModel _mainViewModel;
        [ObservableProperty] private string naziv = string.Empty;


        public string Name
        {
            get
            {
                return _certificateType.Name;
            }
            set
            {
                _certificateType.Name = value;
            }
        }

        [RelayCommand] 
        private void GoToBack()
        {
            if(MainViewModel != null)
            {
                MainViewModel.CurrentPage = MainViewModel.DokumentaPage;
            }
        }


        [RelayCommand]
        private void OpenDocumentTypePanel()
        {

        }

    }
}
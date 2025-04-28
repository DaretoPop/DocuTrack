using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                MainViewModel.GoToDokumenta();
            }
        }

        [RelayCommand]
        private void GoToEdit()
        {
            if (MainViewModel != null)
            {
                MainViewModel.GoToUpsertCertificateType(_certificateType);
            }
        }

        [RelayCommand]
        private void OpenCertificateType()
        {
            if (MainViewModel != null)
            {
                MainViewModel.GoToCertificateType(_certificateType);
            }
        }


        [RelayCommand]
        private async Task DodajZahtevZaSticanje(Window parentWindow)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Files",
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "All Supported Files", Extensions = { "pdf", "doc", "docx", "jpg", "jpeg", "png", "bmp", "gif" } },
                    new FileDialogFilter { Name = "PDF Files", Extensions = { "pdf" } },
                    new FileDialogFilter { Name = "Word Documents", Extensions = { "doc", "docx" } },
                    new FileDialogFilter { Name = "Images", Extensions = { "jpg", "jpeg", "png", "bmp", "gif" } }
                    
                }
            };

            var result = await dialog.ShowAsync(parentWindow);

            if (result != null && result.Any())
            {
                if (_certificateType.Documents == null)
                {
                    _certificateType.Documents = new List<RequestFile>();
                }
                foreach (var filePath in result)
                {
                    // Process each selected file
                    // Example: Add to the CertificateType's Documents collection
                    _certificateType.Documents.Add(new RequestFile
                    {
                        FilePath = filePath,
                        CertificateTypeID = _certificateType.ID,
                        RequestType = RequestTypeEnum.Sticanje // Replace with appropriate enum value
                    });
                }
            }
        }


    }
}
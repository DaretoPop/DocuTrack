using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
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
            CertificateType = type;
            CertificateType.Documents = CertificateType.Documents ?? DatabaseHelper.getCertificateTypeDocuments(CertificateType.ID);
            LoadRequestFiles();
        }

        [ObservableProperty] private CertificateType? certificateType;
        [ObservableProperty] private MainViewModel mainViewModel;
        [ObservableProperty] private string naziv = string.Empty;

        public ObservableCollection<RequestFileViewModel> SticanjeFiles { get; set; } = new();
        public ObservableCollection<RequestFileViewModel> ObnovaFiles { get; set; } = new();
        public ObservableCollection<RequestFileViewModel> RefreshFiles { get; set; } = new();

        public void LoadRequestFiles()
        {
            if (CertificateType?.Documents != null)
            {
                SticanjeFiles = new ObservableCollection<RequestFileViewModel>(
                    CertificateType.Documents
                        .Where(d => d.RequestType == RequestTypeEnum.Sticanje)
                        .Select(d => new RequestFileViewModel( mainViewModel, d ))
                );

                ObnovaFiles = new ObservableCollection<RequestFileViewModel>(
                    CertificateType.Documents
                        .Where(d => d.RequestType == RequestTypeEnum.Obnova)
                        .Select(d => new RequestFileViewModel(mainViewModel, d))
                );

                RefreshFiles = new ObservableCollection<RequestFileViewModel>(
                    CertificateType.Documents
                        .Where(d => d.RequestType == RequestTypeEnum.Refresh)
                        .Select(d => new RequestFileViewModel(mainViewModel, d))
                );
            }
        }

        public string Name
        {
            get => CertificateType?.Name ?? string.Empty;
            set
            {
                if (CertificateType != null)
                {
                    CertificateType.Name = value;
                }
            }
        }

        [RelayCommand]
        private void GoToBack()
        {
            MainViewModel?.GoToDokumenta();
        }

        [RelayCommand]
        private void GoToEdit()
        {
            MainViewModel?.GoToUpsertCertificateType(CertificateType);
        }

        [RelayCommand]
        private void OpenCertificateType()
        {
            MainViewModel?.GoToCertificateType(CertificateType);
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
                if (CertificateType?.Documents == null)
                {
                    CertificateType.Documents = new List<RequestFile>();
                }

                foreach (var filePath in result)
                {
                    CertificateType.Documents.Add(new RequestFile
                    {
                        FilePath = filePath,
                        CertificateTypeID = CertificateType.ID,
                        RequestType = RequestTypeEnum.Sticanje
                    });
                }
            }
        }
    }
}
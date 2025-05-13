using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class CertificateAcquiredViewModel : BaseViewModel
    {

        

        [ObservableProperty]
        public string test = "test from CERTACQ";

        [ObservableProperty]
        private ObservableCollection<CertificateType> certificateTypes = new();

        [ObservableProperty]
        private Certificate certificate = new();

        [ObservableProperty]
        private CertificateType? selectedCertificateType;

        [ObservableProperty] private string errorMessage = "";

        [ObservableProperty] private Sailor sailor = new();

        [ObservableProperty] private List<CertificateFile> files = new();

        [ObservableProperty] private string howManyFilesAreSelected = "";
        [ObservableProperty] private CertificateType preselectedCertificateType = new();
        [ObservableProperty] private bool isObnovi = false;

        public CertificateAcquiredViewModel(Certificate cert, Sailor sailor=null, CertificateType preselectedCertificateType = null)
        {
            Sailor= sailor ?? new Sailor();
            PreselectedCertificateType = preselectedCertificateType ?? new CertificateType();
            if (cert != null)
            {
                Certificate = cert;
                //SelectedCertificateType = DatabaseHelper.GetCertificateTypeByID(cert.CertificateTypeID);
            }
            else
            {
                Certificate = new Certificate
                {
                    DateAcquiredDate = DateTime.Today,
                    DateExpirationDate = DateTime.Today.AddYears(1),
                    CertificateType = PreselectedCertificateType
                };
            }

            IsObnovi = PreselectedCertificateType.ID == 0;
            OnPropertyChanged(nameof(IsObnovi));

            CertificateTypes = new ObservableCollection<CertificateType>(DatabaseHelper.geCertificateTypes());
        }

        [RelayCommand]
        private void SaveCertificate(Window window)
        {
            errorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
            if (IsObnovi)
            {
                if (certificate.CertificateType == null || certificate.CertificateType.ID==0 || certificate.PlaceComboBox == null)
                {
                    errorMessage = "All fields are required.";
                    OnPropertyChanged(nameof(ErrorMessage));
                    return;
                }
            }
            else
            {
                if (certificate.PlaceComboBox == null)
                {
                    errorMessage = "All fields are required.";
                    OnPropertyChanged(nameof(ErrorMessage));
                    return;
                }
            }


            if (isObnovi)
            {
                certificate.CertificateTypeID = certificate.CertificateType.ID;

            }
            else
            {
                certificate.CertificateTypeID = PreselectedCertificateType.ID;
            }
            certificate.SailorID = sailor.ID;
            certificate.DateAcquired = certificate.DateAcquiredDate.Value.ToString("yyyy-MM-dd");
            certificate.DateExpiration = certificate.DateExpirationDate.Value.ToString("yyyy-MM-dd");
            certificate.Place = certificate.PlaceComboBox.Content.ToString();
            
            
            DatabaseHelper.AddCertificate(certificate, files);
            window.Close();

            //DatabaseHelper.AddCertificate(newCertificate);
            //Console.WriteLine("Certificate added successfully.");
        }

        [RelayCommand]
        private async Task DodajDokumenta(Window parentWindow)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Files",
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "All Supported Files", Extensions = { "pdf",  "docx", "jpg", "jpeg", "png", "bmp", "gif" } },
                    new FileDialogFilter { Name = "PDF Files", Extensions = { "pdf" } },
                    new FileDialogFilter { Name = "Word Documents", Extensions = { "docx" } },
                    new FileDialogFilter { Name = "Images", Extensions = { "jpg", "jpeg", "png", "bmp", "gif" } }
                }
            };

            var result = await dialog.ShowAsync(parentWindow);

            if (result != null && result.Any())
            {
                var documentsForAdd = new List<CertificateFile>();

                foreach (var filePath in result)
                {
                    documentsForAdd.Add(new CertificateFile
                    {
                        FilePath = filePath,
                        FileName = Path.GetFileName(filePath),
                        CertificateID = 0
                    });
                }

                files = documentsForAdd;
                OnPropertyChanged(nameof(Files));
                howManyFilesAreSelected = $"{files.Count} files selected";
                OnPropertyChanged(nameof(HowManyFilesAreSelected));

            }
            

        }
    }
}
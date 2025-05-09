using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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


    public partial class SailorViewModel : BaseViewModel

    {
        [ObservableProperty] private Sailor? _sailor;
        [ObservableProperty] private MainViewModel? _mainViewModel;
        [ObservableProperty] private Certificate? _selectedCertificate = null;
        public bool IsCertificateSelected => _selectedCertificate != null;
        public ObservableCollection<Certificate> Certificates { get; set; } = new();
        public ObservableCollection<CertificateFile> ScannedDocuments { get; set; } = new();

        public ObservableCollection<CertificateFile> SelectedFiles { get; set;  } = new();

        public bool? haveMultipleVersions => SelectedCertificate?.Versions?.Any();

        [ObservableProperty] public Certificate selectedCertificateListBox = null;

        partial void OnSelectedCertificateListBoxChanged(Certificate? value)
        {
            SelectedCertificate = value;
            OnPropertyChanged(nameof(SelectedCertificateListBox));
        }




        partial void OnSelectedCertificateChanged(Certificate? value)
        {
            // Notify that IsCertificateSelected has changed
            ScannedDocuments =
                new ObservableCollection<CertificateFile>(DatabaseHelper.getCertificateFilesForSailor(value));
            OnPropertyChanged(nameof(IsCertificateSelected));
            OnPropertyChanged(nameof(ScannedDocuments));
            OnPropertyChanged(nameof(haveMultipleVersions));
        }

        public SailorViewModel()
        {
        }

        private void initializeDocuments()
        {
            ScannedDocuments =
                new ObservableCollection<CertificateFile>(DatabaseHelper.getCertificateFilesForSailor(SelectedCertificate));
            OnPropertyChanged(nameof(ScannedDocuments));
        }

        public SailorViewModel(MainViewModel mainViewModel, Sailor sailor, Certificate selectedCertificate=null)
        {
            _mainViewModel = mainViewModel;
            _sailor = sailor;
            

            initialize();
            SelectedCertificate = selectedCertificate;
            OnPropertyChanged(nameof(SelectedCertificate));
        }

        private void initialize()
        {
           

            //ScannedDocuments = new ObservableCollection<CertificateFile>(mockupList);
            Certificates = new ObservableCollection<Certificate>(DatabaseHelper.GetCertificateForSailor(Sailor));



            OnPropertyChanged(nameof(Sailor));
            OnPropertyChanged(nameof(SelectedCertificate));
            OnPropertyChanged(nameof(ScannedDocuments));
            OnPropertyChanged(nameof(Certificates));

        }




        public string Fullname
        {
            get
            {
                return $"{Sailor?.Name} {Sailor?.Surname}";
            }
        }

        public string GID
        {
            get
            {
                return Sailor?.GID.ToString() ?? string.Empty;
            }
        }

        public string Refresh
        {
            get
            {
                return Sailor?.Refresh ?? string.Empty;
            }
        }





        // Back Button to navigate to ProfilPage or PocentaPage(sertifikati)
        [RelayCommand]
        public void GoBack()
        {
            if (MainViewModel != null)
            {
                if (MainViewModel?.IsPocetnaView == true)
                {
                    MainViewModel.CurrentPage = MainViewModel.PocetnaPage;
                }
                else
                {
                    MainViewModel?.GoToSailors();
                }
            }
        }


        //When you go from SailorRowView to SailorView
        [RelayCommand]
        private void GoToSailorPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToSailorPage(_sailor);

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }

        //When you want to edit
        [RelayCommand]
        private void GoToEditSailorPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToAddEditSailorPage(_sailor);

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }


         [RelayCommand]
        private async void Sticanje(Window parentWindow)
        {
            Console.WriteLine("SticanjeButton");
            initialize();


            // Open the CertificateAcquiredView
            var certificateAcquiredViewModel = new CertificateAcquiredViewModel(null); // Instantiate your ViewModel
            var certificateAcquiredView = new CertificateAcquiredView { DataContext = certificateAcquiredViewModel };

             // Show the window as a dialog asynchronously
            await certificateAcquiredView.ShowDialog(parentWindow);
            
        }   

          [RelayCommand]
        private void Print()
        {
            if (SelectedFiles.Count == 0)
            {
                //No files selected for printing.
                return;
            }

            try
            {
                // Submit selected files to DatabaseHelper for processing
                var outputFilePath = DatabaseHelper.GeneratePrintFile(SelectedFiles);

                // Open the generated file (optional)
                if (!string.IsNullOrEmpty(outputFilePath) && File.Exists(outputFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputFilePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

          [RelayCommand]
        private void ZahtevZaObnovu()
        {
            try
            {
                var outputFilePath = DatabaseHelper.GenerateZahtevi(SelectedCertificate.CertificateTypeID, RequestTypeEnum.Obnova);

                if (!string.IsNullOrEmpty(outputFilePath) && File.Exists(outputFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputFilePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

          [RelayCommand]
        private void ZahtevZaRefresh()
        {
            try
            {
                var outputFilePath = DatabaseHelper.GenerateZahtevi(SelectedCertificate.CertificateTypeID, RequestTypeEnum.Refresh);

                if (!string.IsNullOrEmpty(outputFilePath) && File.Exists(outputFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputFilePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

          [RelayCommand]
        private void ZahtevZaSticanje()
        {
            try
            {
                var outputFilePath = DatabaseHelper.GenerateZahtevi(SelectedCertificate.CertificateTypeID, RequestTypeEnum.Sticanje);

                if (!string.IsNullOrEmpty(outputFilePath) && File.Exists(outputFilePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outputFilePath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during printing: {ex.Message}");
            }
        } 

           [RelayCommand]
        private void OBNOVI()
        {
            Console.WriteLine("OBNOVI DOKUMENT");
        } 
        
           [RelayCommand]
        private async void StareVerzije(Window parentWindow)
        {
            if (SelectedCertificate == null || SelectedCertificate.Versions == null || !SelectedCertificate.Versions.Any())
            {
                Console.WriteLine("No older versions available.");
                return;
            }

            // Create the view and view model for selecting a version
            var selectVersionViewModel = new SelectCertificateVersionViewModel(SelectedCertificate.Versions);
            var selectVersionView = new SelectCertificateVersionView
            {
                DataContext = selectVersionViewModel
            };

            // Show the window as a dialog
            await selectVersionView.ShowDialog(parentWindow);

            // Update the SelectedCertificate with the selected version
            if (selectVersionViewModel.SelectedVersion != null)
            {
                SelectedCertificateListBox = null;
                OnPropertyChanged(nameof(SelectedCertificateListBox));

                SelectedCertificate = selectVersionViewModel.SelectedVersion;
                OnPropertyChanged(nameof(SelectedCertificate));
            }
        } 
        
        

        [RelayCommand]
        private async Task DodajDokument(Window parentWindow)
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
                        CertificateID = SelectedCertificate.ID
                    });
                }

                if (documentsForAdd.Count > 0)
                {
                    DatabaseHelper.addCertificateFiles(documentsForAdd);
                    initializeDocuments();
                }
            }
        }

    }
}
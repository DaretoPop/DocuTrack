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

        public ObservableCollection<CertificateFile> SelectedFiles { get; set; } = new();

        public bool? haveMultipleVersions => SelectedCertificate?.Versions?.Any();
        [ObservableProperty] private string errorMessage = "";

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
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
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

        public SailorViewModel(MainViewModel mainViewModel, Sailor sailor, Certificate selectedCertificate = null)
        {
            _mainViewModel = mainViewModel;
            _sailor = sailor;


            initialize();
            SelectedCertificate = selectedCertificate;
            OnPropertyChanged(nameof(SelectedCertificate));
            BindScaling(mainViewModel);
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
            //Console.WriteLine("SticanjeButton");
            initialize();


            // Open the CertificateAcquiredView
            var certificateAcquiredViewModel = new CertificateAcquiredViewModel(null, Sailor); // Instantiate your ViewModel
            var certificateAcquiredView = new CertificateAcquiredView { DataContext = certificateAcquiredViewModel };

            // Show the window as a dialog asynchronously
            await certificateAcquiredView.ShowDialog(parentWindow);
            initialize();

        }

        [RelayCommand]
        private void Print()
        {
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
            if (SelectedFiles.Count == 0)
            {
                //No files selected for printing.
                ErrorMessage = "Niste izabrali fajlove koje treba da se odstampaju";
                OnPropertyChanged(nameof(ErrorMessage));
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
                else
                {
                    ErrorMessage = "Nije selektovan ni jedan dokument";
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during printing: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage));
                //Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ZahtevZaObnovu()
        {
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
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
                else
                {
                    ErrorMessage = "Nema zahteva za obnovu za ovaj tip dokumenta";
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during printing: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage));
                //Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ZahtevZaRefresh()
        {
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
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
                else
                {
                    ErrorMessage = "Nema zahteva za refresh za ovaj tip dokumenta";
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during printing: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage));
                //Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ZahtevZaSticanje()
        {
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
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
                else
                {
                    ErrorMessage = "Nema zahteva za sticanje za ovaj tip dokumenta";
                    OnPropertyChanged(nameof(ErrorMessage));
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during printing: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage));
                Console.WriteLine($"Error during printing: {ex.Message}");
            }
        }

        [RelayCommand]
        private async void OBNOVI(Window parentWindow)
        {
            // Open the CertificateAcquiredView
            var certificateAcquiredViewModel = new CertificateAcquiredViewModel(null, Sailor, new CertificateType() { ID = SelectedCertificate.CertificateTypeID, Name = SelectedCertificate.Name }); // Instantiate your ViewModel
            var certificateAcquiredView = new CertificateAcquiredView { DataContext = certificateAcquiredViewModel };

            // Show the window as a dialog asynchronously
            await certificateAcquiredView.ShowDialog(parentWindow);
            initialize();
        }

        [RelayCommand]
        private async Task StareVerzije(Window parentWindow)
        {
            if (SelectedCertificate == null || SelectedCertificate.Versions == null || !SelectedCertificate.Versions.Any())
            {
                //Console.WriteLine("No older versions available.");
                ErrorMessage = "No older versions available.";
                OnPropertyChanged(nameof(ErrorMessage));
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
                    ErrorMessage = "";
                    OnPropertyChanged(nameof(ErrorMessage));
                    try
                    {
                        DatabaseHelper.addCertificateFiles(documentsForAdd);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = $"Error during adding files: {ex.Message}";
                        OnPropertyChanged(nameof(ErrorMessage));
                        //Console.WriteLine($"Error during adding files: {ex.Message}");
                    }

                    initializeDocuments();
                }
            }
        }


        [RelayCommand]
        private async Task Delete(Window parentWindow)
        {
            if (MainViewModel == null || _selectedCertificate == null)
                return;

            _selectedCertificate.Sailor = Sailor;
            var dialog = new ConfirmationDialog
            {
                Message = $"Da li ste sigurni da �elite da obri�ete {_selectedCertificate.Name} za {_selectedCertificate.SailorName}?"
            };

            // Show the dialog as a modal window
            var result = await dialog.ShowDialog<bool>(parentWindow);

            if (result)
            {
                ErrorMessage = "";
                OnPropertyChanged(nameof(ErrorMessage));
                try
                {
                    DatabaseHelper.DeleteCertificate(_selectedCertificate);

                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error during deleting files: {ex.Message}";
                    OnPropertyChanged(nameof(ErrorMessage));
                    //Console.WriteLine($"Error during deleting files: {ex.Message}");
                }
                initialize();
            }
        }



        [RelayCommand]
        private async Task DeleteSkenirano(Window parentWindow)
        {
            ErrorMessage = "";
            OnPropertyChanged(nameof(ErrorMessage));
            if (SelectedFiles.Count == 0)
            {
                //No files selected for printing.
                ErrorMessage = "Niste izabrali fajlove koje treba da se izbrisu";
                OnPropertyChanged(nameof(ErrorMessage));
                return;
            }

            try
            {
                DatabaseHelper.DeleteCertificateFiles(SelectedFiles);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during deleting: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage));
            }
            initialize();
        }



    }
}
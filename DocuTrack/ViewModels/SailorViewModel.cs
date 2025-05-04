using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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



        partial void OnSelectedCertificateChanged(Certificate? value)
        {
            // Notify that IsCertificateSelected has changed
            ScannedDocuments =
                new ObservableCollection<CertificateFile>(DatabaseHelper.getCertificateFilesForSailor(value));
            OnPropertyChanged(nameof(IsCertificateSelected));
            OnPropertyChanged(nameof(ScannedDocuments));
        }

        public SailorViewModel()
        {
        }

        public SailorViewModel(MainViewModel mainViewModel, Sailor sailor, Certificate selectedCertificate=null)
        {
            _mainViewModel = mainViewModel;
            _sailor = sailor;
            SelectedCertificate = selectedCertificate;

            initialize();
        }

        private void initialize()
        {
           


            List<RequestFileViewModel> mockupList = new List<RequestFileViewModel>();
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back.png", ID = 1, RequestType = RequestTypeEnum.Obnova }));
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back1.png", ID = 1, RequestType = RequestTypeEnum.Sticanje }));
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back2.png", ID = 1, RequestType = RequestTypeEnum.Sticanje }));
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back3.png", ID = 1, RequestType = RequestTypeEnum.Refresh }));
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back4.png", ID = 1, RequestType = RequestTypeEnum.Obnova }));
            mockupList.Add(new RequestFileViewModel(_mainViewModel, new RequestFile() { CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back4.png", ID = 1, RequestType = RequestTypeEnum.Obnova }));

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
        private void Sticanje()
        {
            Console.WriteLine("SticanjeButton");
            initialize();
            
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
        private void StareVerzije()
        {
            Console.WriteLine("VIDI STARE VERZIJE");
        } 
         
    }
}
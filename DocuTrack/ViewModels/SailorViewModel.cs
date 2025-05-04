using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [ObservableProperty] public bool IsCertificateSelected => _selectedCertificate != null;

        public SailorViewModel()
        {
        }

        public SailorViewModel(MainViewModel mainViewModel, Sailor sailor)
        {
            _mainViewModel = mainViewModel;
            _sailor = sailor;

            //! Obrisi, testirao sam binding
            // ScannedDocuments.Add(new RequestFileViewModel(mainViewModel, new RequestFile{ FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back.png" }));
            // ScannedDocuments.Add(new RequestFileViewModel(mainViewModel, new RequestFile{ FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back.png" }));
            // DocumentIteams.Add(new RequestFileViewModel(mainViewModel, new RequestFile{ FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back.png" }));


            List<RequestFileViewModel> mockupList = new List<RequestFileViewModel>();
            mockupList.Add(new RequestFileViewModel(mainViewModel, new RequestFile() {CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back.png",ID=1, RequestType = RequestTypeEnum.Obnova}));
            mockupList.Add(new RequestFileViewModel(mainViewModel, new RequestFile() {CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back1.png",ID=1, RequestType = RequestTypeEnum.Sticanje}));
            mockupList.Add(new RequestFileViewModel(mainViewModel, new RequestFile() {CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back2.png",ID=1, RequestType = RequestTypeEnum.Sticanje}));
            mockupList.Add(new RequestFileViewModel(mainViewModel, new RequestFile() {CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back3.png",ID=1, RequestType = RequestTypeEnum.Refresh}));
            mockupList.Add(new RequestFileViewModel(mainViewModel, new RequestFile() {CertificateTypeID = 1, FilePath = "/home/pop/docutrack/DocuTrack/Assets/Back4.png",ID=1, RequestType = RequestTypeEnum.Obnova}));

            ScannedDocuments = new ObservableCollection<RequestFileViewModel>(mockupList);
            DocumentIteams = new ObservableCollection<RequestFileViewModel>(mockupList);

            _selectedCertificate = new Certificate()
            {
                CertificateType = new CertificateType() { ID = 0, Name = "Mockup Display" },
                DateAcquired = "27.05.1994", DateExpiration = "27.05.1995", Place = "Kotor", ID = 0,
                CertificateTypeID = 0
            };
            _selectedCertificate = null;
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


     public ObservableCollection<RequestFileViewModel> DocumentIteams { get; set; } = new();
     public ObservableCollection<RequestFileViewModel> ScannedDocuments { get; set; } = new();


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
        }   

          [RelayCommand]
        private void Print()
        {
            Console.WriteLine("Stampaj");
        }

          [RelayCommand]
        private void ZahtevZaObnovu()
        {
            Console.WriteLine("ZahtevzaOBNOVU");
        }

          [RelayCommand]
        private void ZahtevZaRefresh()
        {
            Console.WriteLine("ZahtevzarREFRESH");
        }

          [RelayCommand]
        private void ZahtevZaSticanje()
        {
            Console.WriteLine("ZahtevzaSTICANJE");
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
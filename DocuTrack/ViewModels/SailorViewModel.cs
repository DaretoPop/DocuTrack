using System;
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
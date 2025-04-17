using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
                    MainViewModel?.GoToPomorci();
                }
            }
        }

        [RelayCommand]
        private void GoToSailorPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel?.GoToPomorciPanel();

                if (MainViewModel?.IsPocetnaView != null)
                {
                    MainViewModel.IsPocetnaView = false;
                }
            }
        }




    }
}
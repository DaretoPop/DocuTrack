using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Data;
using DocuTrack.DataModels;
using DocuTrack.Pages;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class SailorsViewModel : BaseViewModel

    {
        public ObservableCollection<Sailor> Sailors { get; set; }
        public SailorsViewModel()
        {
            Sailors = new ObservableCollection<Sailor>(DatabaseHelper.GetSailors());
        }

        public SailorsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Sailors = new ObservableCollection<Sailor>(DatabaseHelper.GetSailors());
        }


        [ObservableProperty] private MainViewModel? _mainViewModel;


        [RelayCommand]
        private void GoToKreirajPomorcaPage()
        {
            if (MainViewModel != null)
            {
                MainViewModel.CurrentPage = MainViewModel.KreirajKorisnikaPage;
            }
        }

        [RelayCommand]
        private void GoToProfilPanelPage()
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

        [RelayCommand]
        private void GoToSailorPage()
        {

        }


    }
} 
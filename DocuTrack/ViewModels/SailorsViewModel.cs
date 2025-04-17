using System.Collections.Generic;
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
        public ObservableCollection<SailorViewModel> Sailors { get; set; }
        [ObservableProperty] private MainViewModel? _mainViewModel;


        public SailorsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Sailors = new ObservableCollection<SailorViewModel>(_getSailors());
        }


        private List<SailorViewModel> _getSailors()
        {
            var sailors = new List<SailorViewModel>();

            var sailorList = DatabaseHelper.GetSailors();
            foreach (var sailor in sailorList)
            {
                sailors.Add(new SailorViewModel(_mainViewModel, sailor));
            }

            return sailors;

        }


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
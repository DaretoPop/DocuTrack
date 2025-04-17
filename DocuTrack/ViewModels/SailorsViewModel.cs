using System.Runtime.Serialization.Formatters;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocuTrack.Views;

namespace DocuTrack.ViewModels
{
    public partial class SailorsViewModel : BaseViewModel

    {


        public SailorsViewModel()
        {
        }

        public SailorsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
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


    }
} 